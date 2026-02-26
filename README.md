# C# Desktop GUI Framework Comparison

> 系統性比較 12 種 C# 桌面應用程式開發框架，透過統一的 Demo 專案量化各框架的差異。

---

## 專案說明

每個 Demo 實作完全相同的功能：

- **Dashboard 頁** — 統計卡片 + 活動列表
- **Controls 頁** — 常用控制項展示（按鈕、文字輸入、滑桿、下拉選單…）
- **Animation 頁** — 動畫效果展示
- **Data 頁** — 資料列表 + 搜尋過濾

UI 風格一致：深色側邊欄 + 頁面導覽，方便並排比較各框架的 API 設計與開發體驗。

---

## Demo 清單

### 原生 C# GUI 框架

| 專案 | 框架 | .NET 版本 | 說明 |
|------|------|-----------|------|
| `WinFormsNetFxDemo` | WinForms | .NET Fx 4.8 | 傳統 WinForms，完全依賴系統 .NET Framework |
| `WinFormsDemo` | WinForms | .NET 10 | 現代化 WinForms，跨版本相容 |
| `WpfDemo` | WPF | .NET 10 | XAML + MVVM，Windows 原生 UI |
| `UwpDemo` | UWP | Windows SDK | AppContainer 沙盒，MSIX 部署，可跨裝置（PC / Xbox / HoloLens） |
| `WinUI3Demo` | WinUI 3 | .NET 10 | 最新 Windows UI 框架，Fluent Design |
| `UnoDemo` | Uno Platform | .NET 8 | 跨平台（Windows / macOS / Linux / iOS / Android / WebAssembly） |
| `MauiDemo` | .NET MAUI | .NET 10 | 微軟官方跨平台框架（Windows / macOS / iOS / Android） |
| `AvaloniaDemo` | Avalonia UI | .NET 8 | 開源跨平台，SkiaSharp 自繪，Linux 支援最佳 |

### Web UI + C# 橋接框架

| 專案 | 框架 | 說明 |
|------|------|------|
| `BlazorHybridDemo` | Blazor Hybrid | Razor + HTML/CSS，透過 WebView 嵌入，官方微軟支援 |
| `WebView2Demo` | WebView2 | 直接嵌入 Chromium Edge，完整 Web 技術棧 |
| `PhotinoDemo` | Photino | 輕量 WebView 橋接，比 Electron 小得多 |
| `CefSharpDemo` | CefSharp | Chromium Embedded Framework，功能最完整的 Web 橋接方案 |

---

## 快速開始

### 環境需求

| 需求 | 用途 |
|------|------|
| .NET 8+ SDK | WinForms / WPF / Avalonia / Uno / CefSharp / Blazor / WebView2 / Photino |
| .NET 10 SDK | WinUI 3 / MAUI |
| .NET Framework 4.8 | WinFormsNetFxDemo（Windows 內建） |
| MAUI workload | `dotnet workload install maui` |
| Visual Studio 2019 + UWP workload | UwpDemo（需 Universal Windows Platform 開發工具） |
| Windows SDK 10.0.19041 | UWP / WinUI 3 / MAUI |

### 一鍵編譯所有 Demo

```bat
build-all.bat
```

或直接執行 PowerShell：

```powershell
.\build-all.ps1              # Release 模式（預設）
.\build-all.ps1 -Config Debug
.\build-all.ps1 -SkipChecks  # 略過環境偵測
```

腳本會自動：
1. 偵測所有必要的 SDK / Runtime / 工具鏈是否安裝
2. 列出缺少的項目及安裝方式
3. 依序編譯所有 Demo（顯示 `[N/12]` 進度）
4. 將所有輸出複製到 `Release\<DemoName>\`
5. 顯示摘要（各項建置時間 + 總計時間）

範例輸出：
```
>> [1/12] WinFormsNetFxDemo
  [OK]  Done -> Release\WinFormsNetFxDemo  (3 files, 0 MB)  [1.2s]
...
  Result: 12 built   0 skipped   0 failed
  Total build time: 5m 23s
  Output: C:\...\Release
```

---

## 比較報告

詳細的量化比較請見 [`comparison_report.md`](./comparison_report.md)，包含：

- Build 輸出大小
- NuGet 套件依賴數量
- 增量 / 冷啟動 Build 時間
- 啟動時間 / 記憶體使用量
- XAML 熱重載支援
- 跨平台能力
- 學習曲線與生態系
- UWP 深度說明（裝置家族 / MSIX 部署 / Runtime 比較）

---

## 測試環境

- **OS**：Windows 11
- **.NET SDK**：10.0.101（可向下相容建置 net8.0 專案）
- **Visual Studio**：2019 Enterprise（UWP 建置用）
- **Windows SDK**：10.0.19041.0
