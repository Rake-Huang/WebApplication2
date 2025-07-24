# Match Controller API 使用示例

## API 端点

POST `/api/Match/updateMatchResult`

### 参数
- `matchId` (int): 比赛ID
- `matchEvent` (MatchEvent): 比赛事件枚举

### MatchEvent 枚举值
1. `HomeGoal` = 1 - 主队进球
2. `AwayGoal` = 2 - 客队进球  
3. `NextPeriod` = 3 - 进入下一个时期(半场)
4. `HomeCancel` = 4 - 取消主队进球
5. `AwayCancel` = 5 - 取消客队进球

## 使用示例

### 1. 主队进球
```bash
curl -X POST "https://localhost:7200/api/Match/updateMatchResult?matchId=1&matchEvent=1"
```
响应: `"1:0 (First Half)"`

### 2. 客队进球
```bash
curl -X POST "https://localhost:7200/api/Match/updateMatchResult?matchId=1&matchEvent=2"
```
响应: `"1:1 (First Half)"`

### 3. 进入下半场
```bash
curl -X POST "https://localhost:7200/api/Match/updateMatchResult?matchId=1&matchEvent=3"
```
响应: `"1:1 (Second Half)"`

### 4. 下半场主队进球
```bash
curl -X POST "https://localhost:7200/api/Match/updateMatchResult?matchId=1&matchEvent=1"
```
响应: `"2:1 (Second Half)"`

### 5. 取消最后一个进球
```bash
curl -X POST "https://localhost:7200/api/Match/updateMatchResult?matchId=1&matchEvent=4"
```
响应: `"1:1 (Second Half)"`

## 数据库格式说明

| Id | MatchResult | 说明 |
|----|-------------|------|
| 1 | "HHA" | 主队2球，客队1球 (上半场) |
| 1 | "HHA;" | 主队2球，客队1球，进入下半场 |
| 1 | "HHA;A" | 主队2球，客队2球 (下半场) |

- H = 主队进球
- A = 客队进球
- ; = 半场分隔符

## 错误处理

### 取消不存在的进球
如果尝试取消不存在的进球，会抛出 `UpdateMatchResultException`

例如：如果当前结果是 "HHA;"，尝试执行 `HomeCancel` 会成功(取消最后一个H)
但如果当前结果是 "AA;"，尝试执行 `HomeCancel` 会抛出异常，因为没有主队进球可以取消。

### 比赛不存在
如果 matchId 不存在，会返回 404 Not Found

## 预设数据

系统预设了3个比赛：
- Match ID 1: 空比赛结果 ""
- Match ID 2: "HHA" (主队2球，客队1球)
- Match ID 3: "HHA;A" (主队2球，客队2球，下半场) 