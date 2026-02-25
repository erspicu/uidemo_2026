# 研究筆記：網頁技術 UI + C# 桌面框架

> 記錄日期：2026-02-25  
> 狀態：待建立 Demo

---

## 概念

用 HTML / CSS / JavaScript 負責 UI 渲染，C# 負責商業邏輯與系統 API，
兩者在同一個桌面程式內溝通（不需要 HTTP Server）。

---

## 候選框架

### 1. Blazor Hybrid（官方 Microsoft 推薦）

- **前端**：Razor 元件（HTML + CSS + C# 混合）
- **C# 通訊**：直接呼叫，同一個 process，無需序列化
- **底層**：WebView2（Chromium）
- **整合方式**：
  - WPF + Blazor → `Microsoft.AspNetCore.Components.WebView.Wpf`
  - WinForms + Blazor → `Microsoft.AspNetCore.Components.WebView.WindowsForms`
  - MAUI Blazor → 官方範本
- **跨平台**：透過 MAUI 可到 iOS / Android / macOS
- **優點**：複用 Blazor Web 元件、.NET 深度整合、官方長期支援
- **缺點**：前端技術限定 Razor，非純 JS 框架

### 2. Photino.NET（輕量跨平台）

- **前端**：React / Vue / Angular / 純 HTML，任意 JS 框架
- **C# 通訊**：JSON 訊息傳遞（`SendWebMessage` / `RegisterWebMessageReceivedHandler`）
- **底層**：OS 原生 WebView（Windows: WebView2，macOS: WKWebView，Linux: WebKitGTK）
- **體積**：極小（~500 KB），不打包 Chromium
- **跨平台**：✅ Windows / macOS / Linux
- **GitHub**：https://github.com/tryphotino/photino.NET
- **優點**：最接近 Electron 體驗，C# 友善，輕量
- **缺點**：社群較小，JS ↔ C# 需自行設計通訊協定

### 3. CefSharp（嵌入完整 Chromium）

- **前端**：任意 Web 技術
- **C# 通訊**：`RegisterJsObject` 直接暴露 C# 物件給 JS，或 `ExecuteScriptAsync`
- **底層**：Chromium Embedded Framework（CEF）
- **體積**：大（~100 MB，含完整 Chrome）
- **跨平台**：❌ 僅 Windows
- **GitHub**：https://github.com/cefsharp/CefSharp
- **優點**：功能最完整、Chrome DevTools 可用、JS 橋接最自然
- **缺點**：僅 Windows，體積龐大

### 4. WebView2 直接使用（最輕量嵌入）

- **前端**：任意 Web 技術
- **C# 通訊**：`PostWebMessageAsString` / `WebMessageReceived` 事件
- **底層**：Microsoft Edge（WebView2，需 Edge 已安裝）
- **整合方式**：WPF / WinForms / WinUI3 / Win32 均可嵌入
- **跨平台**：❌ 僅 Windows
- **優點**：最輕量，OS 共享 Edge Runtime（近乎零體積增加）
- **缺點**：通訊 API 較低階，需自行包裝

---

## 比較表

| 框架 | 前端技術 | C# 通訊 | 體積 | 跨平台 | 難度 |
|------|---------|---------|------|--------|------|
| Blazor Hybrid | Razor/HTML/CSS | 直接呼叫 | 中 | ✅（MAUI） | ⭐⭐ |
| Photino.NET | React/Vue/任意 | JSON 訊息 | 極小 | ✅ | ⭐⭐⭐ |
| CefSharp | 任意 Web | JS 物件橋接 | 大 | ❌ Windows | ⭐⭐⭐ |
| WebView2 + WPF | 任意 Web | PostMessage | 小 | ❌ Windows | ⭐⭐⭐⭐ |

---

## 待建立 Demo

- [ ] Blazor Hybrid (WPF) — 展示 Razor 元件 + C# 直接呼叫
- [ ] Photino.NET + React — 展示 JS 框架 + C# 訊息通訊
- [ ] CefSharp (WPF) — 展示 JS ↔ C# 物件橋接
- [ ] WebView2 + WPF — 展示最輕量嵌入方式

評估指標與之前 GUI 框架比較報告一致：啟動速度、Build 大小、依賴檔案、開發體驗。

---

## 參考資源

- [Blazor Hybrid 官方文件](https://learn.microsoft.com/zh-tw/aspnet/core/blazor/hybrid/)
- [Photino.NET GitHub](https://github.com/tryphotino/photino.NET)
- [CefSharp GitHub](https://github.com/cefsharp/CefSharp)
- [WebView2 官方文件](https://learn.microsoft.com/zh-tw/microsoft-edge/webview2/)
