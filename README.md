# WebApplication2

一個基於 ASP.NET Core 的足球比賽管理與預算查詢 Web API 系統。

## 🏆 功能特色

### ⚽ 足球比賽管理
- **即時比分追蹤** - 使用獨特的字串格式記錄比賽進程
- **比賽事件處理** - 支援進球、換場、取消進球等操作
- **半場管理** - 自動追蹤上半場/下半場狀態
- **進球取消** - 可撤銷最後一個進球記錄

### 💰 預算管理系統
- **月度預算追蹤** - 按月管理預算並計算每日金額
- **跨月查詢** - 支援跨月份、跨年度的預算查詢
- **靈活日期範圍** - 任意日期區間的預算分析

## 🚀 技術架構

- **ASP.NET Core 9.0** - 現代化 Web API 框架
- **記憶體儲存** - 無需資料庫依賴的輕量化設計
- **依賴注入** - 鬆耦合的服務管理
- **OpenAPI/Swagger** - 自動化 API 文件
- **完整測試覆蓋** - NUnit + FluentAssertions + NSubstitute

## 📡 API 端點

### 比賽管理
```
POST /api/Match/updateMatchResult?matchId={id}&matchEvent={event}
```

### 比賽事件類型
| 事件 | 值 | 說明 |
|------|----|----|
| `HomeGoal` | 1 | 主隊進球 |
| `AwayGoal` | 2 | 客隊進球 |
| `NextPeriod` | 3 | 進入下一時期（半場） |
| `HomeCancel` | 4 | 取消主隊進球 |
| `AwayCancel` | 5 | 取消客隊進球 |

## 🎯 比賽結果格式

系統使用獨特的字串格式來記錄比賽狀態：

| 格式 | 說明 |
|------|------|
| `"HHA"` | 主隊 2 球，客隊 1 球（上半場） |
| `"HHA;"` | 主隊 2 球，客隊 1 球，已進入下半場 |
| `"HHA;A"` | 主隊 2 球，客隊 2 球（下半場） |

- `H` = 主隊進球
- `A` = 客隊進球  
- `;` = 半場分隔符

## 🏃‍♂️ 快速開始

### 1. 克隆專案
```bash
git clone https://github.com/Rake-Huang/WebApplication2.git
cd WebApplication2
```

### 2. 運行應用程式
```bash
dotnet run --project WebApplication2
```

### 3. 訪問 API 文件
- **HTTP**: http://localhost:5069/swagger
- **HTTPS**: https://localhost:7283/swagger

## 🧪 運行測試

```bash
# 運行所有測試
dotnet test

# 運行特定測試專案
dotnet test WebApplication2.Tests

# 產生測試覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

## 📁 專案結構

```
WebApplication2/
├── WebApplication2/                 # 主要 Web API 專案
│   ├── Controllers/                 # API 控制器
│   │   └── MatchController.cs      # 比賽管理控制器
│   ├── Services/                   # 業務邏輯層
│   │   ├── MatchService.cs         # 比賽服務
│   │   └── BudgetService.cs        # 預算服務
│   ├── Models/                     # 資料模型
│   │   ├── Match.cs                # 比賽模型
│   │   ├── MatchEvent.cs           # 比賽事件枚舉
│   │   ├── Budget.cs               # 預算模型
│   │   └── Period.cs               # 時期模型
│   ├── Repositories/               # 資料存取層
│   │   ├── IMatchRepository.cs     # 比賽倉庫介面
│   │   ├── InMemoryMatchRepository.cs
│   │   └── IBudgetRepo.cs          # 預算倉庫介面
│   ├── Interfaces/                 # 介面定義
│   ├── Exceptions/                 # 自定義例外
│   └── Program.cs                  # 應用程式進入點
├── WebApplication2.Tests/          # 測試專案
│   ├── MatchServiceTests.cs        # 比賽服務測試
│   ├── MatchControllerTests.cs     # 控制器測試
│   └── BudgetServiceTests.cs       # 預算服務測試
└── API_USAGE_EXAMPLES.md          # API 使用範例
```

## 💡 使用範例

### 比賽操作範例

```bash
# 主隊進球
curl -X POST "http://localhost:5069/api/Match/updateMatchResult?matchId=1&matchEvent=1"
# 回應: "1:0 (First Half)"

# 客隊進球
curl -X POST "http://localhost:5069/api/Match/updateMatchResult?matchId=1&matchEvent=2"
# 回應: "1:1 (First Half)"

# 進入下半場
curl -X POST "http://localhost:5069/api/Match/updateMatchResult?matchId=1&matchEvent=3"
# 回應: "1:1 (Second Half)"

# 取消最後進球
curl -X POST "http://localhost:5069/api/Match/updateMatchResult?matchId=1&matchEvent=4"
```

## 🔧 開發環境需求

- **.NET 9.0 SDK** 或更高版本
- **Visual Studio 2022** 或 **VS Code**（可選）
- **Git**（用於版本控制）

## 🧩 設計模式

- **Repository Pattern** - 資料存取抽象化
- **Service Layer** - 業務邏輯分離
- **Dependency Injection** - 依賴反轉
- **Factory Pattern** - Period 物件創建
- **Custom Exceptions** - 特定錯誤處理

## 🚨 錯誤處理

### 取消不存在的進球
當嘗試取消不存在的進球時，系統會拋出 `UpdateMatchResultException`：

```csharp
// 如果當前結果是 "AA;"，嘗試執行 HomeCancel 會拋出異常
// 因為沒有主隊進球可以取消
```

### 無效的比賽 ID
如果 matchId 不存在，API 會返回適當的錯誤回應。

## 🤝 貢獻指南

1. Fork 此專案
2. 創建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

## 📄 授權條款

此專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案。

## 📞 聯絡資訊

專案連結: [https://github.com/Rake-Huang/WebApplication2](https://github.com/Rake-Huang/WebApplication2) 