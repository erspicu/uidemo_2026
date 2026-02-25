# C# GUI 框架技術評估比較報告

> 測試環境：Windows 11 · .NET SDK 10.0.101 · Visual Studio 2019 Enterprise  
> 測試時間：2025 年  
> 所有 Demo 功能一致：Dashboard / Controls / Animation / Data 四頁，深色側邊欄導覽

---

## 1. Build 輸出大小（Debug 模式）

| 框架 | 輸出目錄大小 | 檔案數 | 主程式 (.exe) 大小 | 備註 |
|------|:-----------:|:------:|:-----------------:|------|
| **WinForms (.NET Fx 4.8)** | ~0 MB | 3 | 25 KB | 完全依賴系統 .NET Framework |
| **WinForms (.NET 8)** | 0.2 MB | 5 | 148 KB | Framework-dependent，依賴系統 .NET 8 Runtime |
| **WPF (.NET 8)** | 0.2 MB | 5 | 148 KB | Framework-dependent，依賴系統 .NET 8 Runtime |
| **WinUI 3 (.NET 10)** | 38.9 MB | 66 | 160 KB | 含 WindowsAppSDK 部分 Runtime |
| **Uno Platform (.NET 8)** | 89.7 MB | 292 | 80 KB | 含 WinAppSDK + Uno Runtime |
| **MAUI (.NET 10)** | 104.1 MB | 419 | 281 KB | 含大量平台工具與圖片處理資源 |
| **Avalonia (.NET 8)** | 128.8 MB | 63 | 148 KB | 含 SkiaSharp 多平台 native 二進位 |

> **Release 模式注意**：啟用 `PublishSingleFile` 或 `SelfContained=true` 後，WPF / WinForms 的大小會暴增至 60–100 MB，而 Avalonia / WinUI 3 等的大小幾乎不變甚至略減（已打包）。

---

## 2. NuGet 套件依賴數量（直接 + 遞移）

| 框架 | NuGet 套件數 |
|------|:-----------:|
| WinForms (.NET Fx 4.8) | 0（全靠 GAC / 系統 DLL） |
| WinForms (.NET 8) | 0（WinForms 內建於 .NET Runtime） |
| WPF (.NET 8) | 0（WPF 內建於 .NET Runtime） |
| WinUI 3 | 14 |
| MAUI | 26 |
| Uno Platform | 20 |
| Avalonia | 30 |

---

## 3. 增量 Build 時間（Incremental Build，已有 obj/ 快取）

| 框架 | 增量 Build 時間 |
|------|:--------------:|
| WinForms (.NET Fx 4.8) | ~1.1 s |
| WinForms (.NET 8) | ~1.6 s |
| WPF (.NET 8) | ~2.2 s |
| Avalonia (.NET 8) | ~6.2 s |
| MAUI (.NET 10) | ~7.7 s |
| WinUI 3 (.NET 10) | ~9.3 s |
| Uno Platform (.NET 8) | ~34.3 s |

### 首次（冷）Build 時間（含 NuGet 還原）

| 框架 | 首次 Build 估計 |
|------|:--------------:|
| WinForms (.NET Fx 4.8) | 2–5 s |
| WinForms (.NET 8) | 3–5 s |
| WPF (.NET 8) | 3–8 s |
| Avalonia (.NET 8) | 30–60 s |
| MAUI (.NET 10) | 10–20 min（含 MSIX 打包、圖片 resizer） |
| WinUI 3 (.NET 10) | 3–5 min（首次需下載 WindowsAppSDK） |
| Uno Platform (.NET 8) | 5–10 min（WinAppSDK + PRI 生成） |

---

## 4. 啟動速度（應用程式首次冷啟動）

| 框架 | 冷啟動體感 | 說明 |
|------|:----------:|------|
| WinForms (.NET Fx 4.8) | ⚡ 極快（< 0.5 s） | 直接使用系統已載入的 CLR |
| WinForms (.NET 8) | ⚡ 極快（< 0.5 s） | .NET 8 JIT 啟動快 |
| WPF (.NET 8) | ⚡ 快（< 1 s） | XAML 初始化略有額外開銷 |
| WinUI 3 (.NET 10) | 🟡 中等（1–2 s） | WinAppRuntime 初始化 |
| Avalonia (.NET 8) | 🟡 中等（1–2 s） | SkiaSharp 初始化 + GPU 準備 |
| MAUI (.NET 10) | 🔴 較慢（2–4 s） | 多層 Handler 架構初始化 |
| Uno Platform (.NET 8) | 🔴 較慢（2–4 s） | WinAppSDK + Uno 底層初始化 |

> 以上為 Debug 模式體感；Release + ReadyToRun 可有效縮短 JIT 啟動時間，WPF/WinForms 可再快 30–50%。

---

## 5. Runtime 相依性

| 框架 | 需要預先安裝 | 可 Self-Contained |
|------|:-----------:|:-----------------:|
| WinForms (.NET Fx 4.8) | .NET Framework 4.8（Windows 11 內建） | ❌ 不支援 |
| WinForms (.NET 8) | .NET 8 Runtime | ✅ 支援 |
| WPF (.NET 8) | .NET 8 Runtime | ✅ 支援 |
| Avalonia (.NET 8) | 無（SkiaSharp 已內含） | ✅ 支援 |
| MAUI (.NET 10) | .NET 10 Runtime（或 MSIX 自帶） | ✅ 支援 |
| WinUI 3 (.NET 10) | Windows App Runtime 1.6+ | ✅ Self-Contained 可行 |
| Uno Platform (.NET 8) | Windows App Runtime 1.6+ | ✅ `WindowsAppSDKSelfContained=true` |

---

## 6. 跨平台支援

| 框架 | Windows | macOS | Linux | iOS | Android | Web |
|------|:-------:|:-----:|:-----:|:---:|:-------:|:---:|
| WinForms (.NET Fx 4.8) | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| WinForms (.NET 8) | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| WPF (.NET 8) | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| WinUI 3 | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| MAUI | ✅ | ✅ | ❌ | ✅ | ✅ | ❌ |
| Avalonia | ✅ | ✅ | ✅ | ✅ | ✅ | ✅（預覽） |
| Uno Platform | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |

---

## 7. UI 精緻度 / 功能豐富度

| 框架 | 動畫支援 | 自定義樣式 | 控制項生態 | 備註 |
|------|:--------:|:---------:|:---------:|------|
| WinForms (.NET Fx 4.8) | 🔴 極弱 | 🔴 困難（GDI+ 手繪） | 🟡 基礎 | 雙緩衝可減少閃爍，但開發繁瑣 |
| WinForms (.NET 8) | 🔴 極弱 | 🔴 困難（GDI+ 手繪） | 🟡 基礎 | 同上，但享有 .NET 8 效能改進 |
| WPF (.NET 8) | ✅ 完整 | ✅ 豐富（XAML Style/Template） | ✅ 豐富 | MVVM 友善，動畫流暢 |
| WinUI 3 | ✅ 完整 | ✅ WinUI Design Language | ✅ WinUI Gallery | 與 Windows 11 UI 原生一致 |
| MAUI | ✅ 完整 | 🟡 中等（平台差異大） | 🟡 中等 | 跨平台 Handler 造成細節不一致 |
| Avalonia | ✅ 完整 | ✅ CSS-like Style | ✅ 豐富 | SkiaSharp 渲染，跨平台表現一致 |
| Uno Platform | ✅ 完整 | ✅ WinUI 相容 XAML | ✅ WinUI 相容 | 對熟悉 WinUI/UWP 的開發者友善 |

---

## 8. 已知問題 / 注意事項

| 框架 | 問題 |
|------|------|
| WinForms | 動畫需手動 GDI+ 雙緩衝，否則嚴重閃爍；無原生深色模式支援 |
| WinForms NetFx | 僅限 Windows；無 nullable 注釋；GAC 版本耦合 |
| WPF | 僅限 Windows；XAML 語法冗長；舊版 DataGrid 效能差 |
| WinUI 3 | 需要 Windows App Runtime；工具鏈複雜（MSBuild 路徑問題）；Debug sidebar 資源色彩覆寫需手動處理 |
| MAUI | 首次 Build 極慢（含 MSIX + 圖片處理）；Animation 頁面在 Debug 模式可能卡頓 |
| Avalonia | Debug 輸出含多平台 native DLL 導致體積龐大；SkiaSharp GPU 初始化略慢 |
| Uno Platform | Build 最慢（WinAppSDK PRI 生成）；需指定 `/p:Platform=x64`；套件版本相容性複雜（Uno 5.x vs 6.x） |

---

## 9. 綜合選用建議

| 情境 | 建議框架 |
|------|---------|
| 純 Windows 桌面，追求最小體積 | **WPF .NET 8** |
| 純 Windows 桌面，追求最快啟動 | **WinForms .NET 8** |
| Windows 11 原生風格 UI | **WinUI 3** |
| Windows 桌面 + 少量跨平台需求 | **Avalonia** |
| 行動裝置（iOS/Android）+ Windows | **MAUI** |
| 真正的全平台（含 Web/Linux） | **Uno Platform** 或 **Avalonia** |
| 維護舊有企業系統 | **WinForms .NET Fx 4.8** |

---

## 附錄：測試專案結構

```
demo1/
├── AvaloniaDemo/        # Avalonia .NET 8
├── MauiDemo/            # .NET MAUI .NET 10
├── WpfDemo/             # WPF .NET 8
├── WinUI3Demo/          # WinUI 3 .NET 10
├── UnoDemo/             # Uno Platform .NET 8 (x64)
├── WinFormsDemo/        # WinForms .NET 8
└── WinFormsNetFxDemo/   # WinForms .NET Framework 4.8
```

各 Demo 均包含：Dashboard / Controls（按鈕、輸入框、進度條、滑桿、清單）/ Animation（緩動動畫）/ Data（統計圖表）四個頁面，並採深色側邊欄導覽設計。
