# build-all.ps1
# Detects missing SDKs/runtimes for all demos, builds each one,
# and copies output to Release\<DemoName>\
#
# Usage:
#   .\build-all.ps1               # Release mode (default)
#   .\build-all.ps1 -Config Debug # Debug mode
#   .\build-all.ps1 -SkipChecks   # Skip env detection, try to build anyway

param(
    [string]$Config = "Release",
    [switch]$SkipChecks
)

Set-StrictMode -Off
$ErrorActionPreference = "Continue"
$Root       = $PSScriptRoot
$ReleaseDir = Join-Path $Root "Release"

# ── Helpers ──────────────────────────────────────────────────────────────────
function Write-Header($msg) {
    $line = "-" * 64
    Write-Host "`n$line" -ForegroundColor Cyan
    Write-Host "  $msg" -ForegroundColor Cyan
    Write-Host $line   -ForegroundColor Cyan
}
function Write-OK($msg)   { Write-Host "  [OK]  $msg" -ForegroundColor Green  }
function Write-Warn($msg) { Write-Host "  [!!]  $msg" -ForegroundColor Yellow }
function Write-Fail($msg) { Write-Host "  [XX]  $msg" -ForegroundColor Red    }
function Write-Info($msg) { Write-Host "        $msg" -ForegroundColor DarkGray }
function Write-Step($msg) { Write-Host "`n>> $msg" -ForegroundColor Magenta   }

# ── 1. Requirement Detection ─────────────────────────────────────────────────
$req = @{}

if (-not $SkipChecks) {
    Write-Header "Detecting Requirements"

    # .NET SDKs
    $sdkLines = & dotnet --list-sdks 2>$null
    $sdkText  = ($sdkLines) -join "`n"

    $maxVer = ($sdkLines | ForEach-Object {
        if ($_ -match '^(\d+)\.') { [int]$Matches[1] }
    } | Measure-Object -Maximum).Maximum

    $req.DotNet8  = ($maxVer -ge 8)   # .NET 10 SDK can build net8.0 targets
    $req.DotNet10 = ($sdkText -match '(?m)^10\.')

    if ($req.DotNet8) {
        $verList = $sdkLines | ForEach-Object { ($_ -split ' ')[0] }
        Write-OK "dotnet SDK installed: $($verList -join ', ')  (can build net8.0+)"
    } else {
        Write-Fail "No .NET 8+ SDK found  -> install: https://dotnet.microsoft.com/download/dotnet/8.0"
    }

    if ($req.DotNet10) { Write-OK  ".NET 10 SDK confirmed" }
    else               { Write-Fail ".NET 10 SDK not found -> https://dotnet.microsoft.com/download/dotnet/10.0" }

    # .NET Framework 4.8
    $nfx = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" -ErrorAction SilentlyContinue
    $req.NetFx48 = ($nfx -and ($nfx.Release -ge 528040))
    if ($req.NetFx48) { Write-OK  ".NET Framework 4.8 installed (Release=$($nfx.Release))" }
    else              { Write-Fail ".NET Framework 4.8 not found -> check Windows Update (built-in on Win11)" }

    # dotnet workloads
    $wlText = (& dotnet workload list 2>$null) -join "`n"
    $req.MauiWL = ($wlText -match 'maui')
    if ($req.MauiWL) { Write-OK  "maui workload installed" }
    else             { Write-Warn "maui workload missing  -> run: dotnet workload install maui" }

    # Visual Studio 2019 + UWP workload
    $vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
    $vs2019  = $null
    if (Test-Path $vswhere) {
        # Prefer VS2019 path that already has UWP targets (SSMS/BuildTools may appear in vswhere output)
        $vs2019 = (& $vswhere -version 16 -products * -property installationPath 2>$null) |
                  Where-Object { $_ -match "Microsoft Visual Studio" -and
                                 (Test-Path (Join-Path $_ "MSBuild\Microsoft\WindowsXaml")) } |
                  Select-Object -First 1
        if (-not $vs2019) {
            # Fallback: any VS2019-range path that looks like a real VS install
            $vs2019 = (& $vswhere -version 16 -products * -property installationPath 2>$null) |
                      Where-Object { $_ -match "Microsoft Visual Studio" } |
                      Select-Object -First 1
        }
    }
    $req.VS2019    = ($null -ne $vs2019) -and (Test-Path $vs2019)
    $uwpTargets    = if ($req.VS2019) { Join-Path $vs2019 "MSBuild\Microsoft\WindowsXaml" } else { "" }
    $req.VS2019UWP = $req.VS2019 -and (Test-Path $uwpTargets)

    if ($req.VS2019UWP)  { Write-OK  "VS2019 + UWP workload found: $vs2019" }
    elseif ($req.VS2019) { Write-Warn "VS2019 found but UWP workload missing -> add 'Universal Windows Platform development' in VS Installer" }
    else                 { Write-Fail "VS2019 not found -> install VS2019 with UWP workload (required for UwpDemo)" }

    # Windows SDK 10.0.19041
    $wsdkPath     = "C:\Program Files (x86)\Windows Kits\10\References\10.0.19041.0"
    $req.WinSDK   = Test-Path $wsdkPath
    if ($req.WinSDK) { Write-OK  "Windows SDK 10.0.19041.0 found" }
    else             { Write-Warn "Windows SDK 10.0.19041.0 not found -> install via VS Installer or standalone SDK" }

    # Summary of missing items
    $missing = @()
    if (-not $req.DotNet8)   { $missing += "  dotnet 8+ SDK : https://dotnet.microsoft.com/download/dotnet/8.0" }
    if (-not $req.DotNet10)  { $missing += "  dotnet 10 SDK : https://dotnet.microsoft.com/download/dotnet/10.0" }
    if (-not $req.MauiWL)    { $missing += "  MAUI workload : dotnet workload install maui" }
    if (-not $req.VS2019UWP) { $missing += "  VS2019 UWP    : install VS2019 with 'Universal Windows Platform development'" }
    if ($missing.Count -gt 0) {
        Write-Host ""
        Write-Host "  ITEMS TO INSTALL:" -ForegroundColor Yellow
        $missing | ForEach-Object { Write-Host $_ -ForegroundColor Yellow }
    }
}
else {
    Write-Warn "Skipping requirement checks (-SkipChecks). Build errors will surface naturally."
    $vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
    $vs2019  = if (Test-Path $vswhere) {
        (& $vswhere -version 16 -products * -property installationPath 2>$null) |
        Where-Object { $_ -match "Microsoft Visual Studio" } | Select-Object -First 1
    } else { $null }
    "DotNet8","DotNet10","NetFx48","MauiWL","VS2019","VS2019UWP","WinSDK" | ForEach-Object { $req[$_] = $true }
}

$msbuild2019 = if ($req.VS2019 -and $vs2019) { Join-Path $vs2019 "MSBuild\Current\Bin\MSBuild.exe" } else { $null }

# ── 2. Demo Build Config Table ────────────────────────────────────────────────
# Tool values:
#   dotnet          -> dotnet build with -o (direct output)
#   dotnet-platform -> dotnet build + /p:Platform=, then find & copy bin output
#   msbuild-uwp     -> VS2019 MSBuild, copy bin\x86 + AppPackages
# ─────────────────────────────────────────────────────────────────────────────
$demos = @(
    @{ Name="WinFormsNetFxDemo";  Csproj="WinFormsNetFxDemo\WinFormsNetFxDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8","NetFx48");
       Hint="Install .NET 8+ SDK and ensure .NET Framework 4.8 is present (built-in on Win11)" },

    @{ Name="WinFormsDemo";       Csproj="WinFormsDemo\WinFormsDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="WpfDemo";            Csproj="WpfDemo\WpfDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="AvaloniaDemo";       Csproj="AvaloniaDemo\AvaloniaDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="MauiDemo";           Csproj="MauiDemo\MauiDemo.csproj";
       Tool="dotnet";            ExtraArgs=@("-f","net10.0-windows10.0.19041.0"); Platform=$null; TFMPat=$null;
       Requires=@("DotNet10","MauiWL");
       Hint="dotnet workload install maui  +  https://dotnet.microsoft.com/download/dotnet/10.0" },

    @{ Name="WinUI3Demo";         Csproj="WinUI3Demo\WinUI3Demo.csproj";
       Tool="dotnet-platform";   ExtraArgs=@();            Platform="x86"; TFMPat="net10\.0-windows";
       Requires=@("DotNet10");   Hint="https://dotnet.microsoft.com/download/dotnet/10.0" },

    @{ Name="UnoDemo";            Csproj="UnoDemo\UnoDemo.Windows\UnoDemo.Windows.csproj";
       Tool="dotnet-platform";   ExtraArgs=@();            Platform="x64"; TFMPat="net8\.0-windows";
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="UwpDemo";            Csproj="UwpDemo\UwpDemo.csproj";
       Tool="msbuild-uwp";       ExtraArgs=@();            Platform="x86"; TFMPat=$null;
       Requires=@("VS2019UWP");
       Hint="Install VS2019 with 'Universal Windows Platform development' workload" },

    @{ Name="BlazorHybridDemo";   Csproj="BlazorHybridDemo\BlazorHybridDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="WebView2Demo";       Csproj="WebView2Demo\WebView2Demo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="PhotinoDemo";        Csproj="PhotinoDemo\PhotinoDemo.csproj";
       Tool="dotnet";            ExtraArgs=@();            Platform=$null; TFMPat=$null;
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" },

    @{ Name="CefSharpDemo";       Csproj="CefSharpDemo\CefSharpDemo.csproj";
       Tool="dotnet-platform";   ExtraArgs=@();            Platform="x64"; TFMPat="net8\.0-windows";
       Requires=@("DotNet8");    Hint="https://dotnet.microsoft.com/download/dotnet/8.0" }
)

# ── 3. Helpers ────────────────────────────────────────────────────────────────
function Find-BinOutput([string]$projDir, [string]$cfg, [string]$plat, [string]$tfmPat) {
    $base = Join-Path $projDir "bin"
    if ($plat) { $base = Join-Path $base $plat }
    $base = Join-Path $base $cfg
    if (-not (Test-Path $base)) { return $null }

    $tfmDir = Get-ChildItem $base -Directory |
              Where-Object { $_.Name -match $tfmPat } | Select-Object -First 1
    if (-not $tfmDir) { return $null }

    # Some SDK builds nest output under a RID subdirectory (e.g., win-x86)
    $ridDir = Get-ChildItem $tfmDir.FullName -Directory |
              Where-Object { $_.Name -match '^win' } | Select-Object -First 1
    if ($ridDir) { return $ridDir.FullName } else { return $tfmDir.FullName }
}

function Get-OutputStats([string]$dir) {
    $files = (Get-ChildItem $dir -Recurse -File -ErrorAction SilentlyContinue).Count
    $bytes = (Get-ChildItem $dir -Recurse -ErrorAction SilentlyContinue |
              Measure-Object Length -Sum).Sum
    return @{ Files=$files; MB=[math]::Round($bytes/1MB,1) }
}

# ── 4. Build Each Demo ────────────────────────────────────────────────────────
Write-Header "Building All Demos  [Config: $Config]"
New-Item $ReleaseDir -ItemType Directory -Force | Out-Null

$results = [System.Collections.Generic.List[hashtable]]::new()

foreach ($d in $demos) {
    Write-Step $d.Name

    $outDir  = Join-Path $ReleaseDir $d.Name
    $csproj  = Join-Path $Root $d.Csproj
    $projDir = Split-Path $csproj -Parent

    # csproj existence
    if (-not (Test-Path $csproj)) {
        Write-Fail "Project file not found: $csproj"
        $results.Add(@{Name=$d.Name; Status="SKIP"; Reason="csproj not found"})
        continue
    }

    # requirement check
    if (-not $SkipChecks) {
        $miss = $d.Requires | Where-Object { -not $req[$_] }
        if ($miss) {
            Write-Warn "Skipped  -- missing: $($miss -join ', ')"
            Write-Info "Fix: $($d.Hint)"
            $results.Add(@{Name=$d.Name; Status="SKIP"; Reason="Missing: $($miss -join ', ')"})
            continue
        }
    }

    # clean output dir
    if (Test-Path $outDir) { Remove-Item $outDir -Recurse -Force }

    $ok  = $false
    $err = @()

    switch ($d.Tool) {

        "dotnet" {
            $args = @("build", $csproj, "-c", $Config, "-o", $outDir) + $d.ExtraArgs
            Write-Info "dotnet $($args -join ' ')"
            $out = & dotnet @args 2>&1
            $ok  = ($LASTEXITCODE -eq 0)
            if (-not $ok) { $err = $out | Where-Object { $_ -match '\serror\s' } | Select-Object -Last 8 }
        }

        "dotnet-platform" {
            $plat = $d.Platform
            $args = @("build", $csproj, "-c", $Config, "/p:Platform=$plat") + $d.ExtraArgs
            Write-Info "dotnet $($args -join ' ')"
            $out = & dotnet @args 2>&1
            $ok  = ($LASTEXITCODE -eq 0)

            if ($ok) {
                $src = Find-BinOutput $projDir $Config $plat $d.TFMPat
                if ($src) {
                    New-Item $outDir -ItemType Directory -Force | Out-Null
                    Copy-Item "$src\*" $outDir -Recurse -Force
                    Write-Info "Copied from: $src"
                } else {
                    # Fallback: find any .exe under bin
                    $exes = Get-ChildItem $projDir -Recurse -Filter "*.exe" |
                            Where-Object { $_.FullName -match "\\$Config\\" -and $_.FullName -notmatch "\\ref\\" } |
                            Select-Object -First 1
                    if ($exes) {
                        $src = Split-Path $exes.FullName
                        New-Item $outDir -ItemType Directory -Force | Out-Null
                        Copy-Item "$src\*" $outDir -Recurse -Force
                        Write-Info "Fallback copy from: $src"
                    } else {
                        Write-Warn "Could not locate bin output directory"
                    }
                }
            } else {
                $err = $out | Where-Object { $_ -match '\serror\s' } | Select-Object -Last 8
            }
        }

        "msbuild-uwp" {
            if (-not $msbuild2019) {
                Write-Fail "VS2019 MSBuild not available"
                $results.Add(@{Name=$d.Name; Status="SKIP"; Reason="VS2019 MSBuild not found"})
                continue
            }
            $plat = $d.Platform
            $args = @(
                $csproj,
                "/t:Restore,Build",
                "/p:Configuration=$Config",
                "/p:Platform=$plat",
                "/p:VisualStudioVersion=16.0",
                "/v:minimal"
            ) + $d.ExtraArgs
            Write-Info "msbuild $($args -join ' ')"
            $out = & $msbuild2019 @args 2>&1
            $ok  = ($LASTEXITCODE -eq 0)

            if ($ok) {
                New-Item $outDir -ItemType Directory -Force | Out-Null
                $binSrc = Join-Path $projDir "bin\$plat\$Config"
                if (Test-Path $binSrc) { Copy-Item "$binSrc\*" $outDir -Recurse -Force }
                $msixSrc = Join-Path $projDir "AppPackages"
                if (Test-Path $msixSrc) {
                    Copy-Item $msixSrc (Join-Path $outDir "AppPackages") -Recurse -Force
                    Write-Info "MSIX package copied to: $outDir\AppPackages"
                }
            } else {
                $err = $out | Where-Object { $_ -match '\serror\s' } | Select-Object -Last 8
            }
        }
    }

    if ($ok) {
        $stats = Get-OutputStats $outDir
        Write-OK "Done -> $outDir  ($($stats.Files) files, $($stats.MB) MB)"
        $results.Add(@{Name=$d.Name; Status="OK"; Files=$stats.Files; MB=$stats.MB})
    } else {
        Write-Fail "Build FAILED"
        $err | ForEach-Object { Write-Info $_ }
        if ((Test-Path $outDir) -and -not (Get-ChildItem $outDir -ErrorAction SilentlyContinue)) {
            Remove-Item $outDir -Force
        }
        $results.Add(@{Name=$d.Name; Status="FAIL"; Reason="build error"})
    }
}

# ── 5. Summary ────────────────────────────────────────────────────────────────
Write-Header "Build Summary"

foreach ($r in $results) {
    $n = $r.Name.PadRight(24)
    switch ($r.Status) {
        "OK"   { Write-OK   "$n $($r.Files) files  $($r.MB) MB" }
        "SKIP" { Write-Warn "$n Skipped : $($r.Reason)" }
        "FAIL" { Write-Fail "$n FAILED" }
    }
}

$nOK   = ($results | Where-Object { $_.Status -eq "OK"   }).Count
$nSkip = ($results | Where-Object { $_.Status -eq "SKIP" }).Count
$nFail = ($results | Where-Object { $_.Status -eq "FAIL" }).Count

Write-Host ""
Write-Host ("  Result: {0} built   {1} skipped   {2} failed" -f $nOK, $nSkip, $nFail) -ForegroundColor Cyan
Write-Host "  Output: $ReleaseDir" -ForegroundColor Cyan
Write-Host ""
