# ResidentCleaner

A repair tool for Cities: Skylines II saves that crash a few seconds after the simulation starts.

天際線 2 的存檔修復工具，適用於「載入存檔後一開始模擬，幾秒內就閃退」的情況。

[Paradox Mods](https://mods.paradoxplaza.com/mods/165656/Windows)

---

## English

### What it does

Some saves end up with cims (residents currently outside their buildings) in a broken state, for example after a vehicle mod is removed, a pathfinding or AI mod is disabled, or public transport vehicles are deleted while passengers are on board. When the game updates these cims, it can crash without any useful error in `Player.log`.

ResidentCleaner adds two buttons to the options menu:

| Button | What it deletes |
| --- | --- |
| Clean broken residents | Only cims that reference vehicles, lanes or targets that no longer exist |
| Clean all residents | Every cim currently outside (use this if the first button is not enough) |

Only the "outside" copies of cims are deleted. Citizens, households and homes are kept, and cims leave their buildings again normally.

### How to use

1. Back up your save.
2. Turn on **Start Paused** in the gameplay options, then load the save.
3. Press Esc, open **Options**, then **ResidentCleaner**, and press one of the buttons.
4. Save as a new file, unpause, and check that the city runs normally.

The result is written to `Logs/ResidentCleaner.log`, for example:

```
ResidentCleaner: checked 60147 residents, deleted 94 (deleteAll = False)
```

You do not need to keep this mod enabled. Turn it on only when you need to repair a save.

### Building from source

Requirements:

- Cities: Skylines II with the official modding toolchain installed (in game: Options, Modding)
- .NET SDK

The project references two extra game assemblies in `ResidentCleaner.csproj`, in addition to the template defaults: `Colossal.Mathematics` and `Colossal.Localization`.

The toolchain post processor needs the .NET 6 runtime. If only a newer runtime is installed, allow roll forward before building:

```powershell
$env:DOTNET_ROLL_FORWARD="Major"
dotnet build
```

The built mod is copied to `%USERPROFILE%\AppData\LocalLow\Colossal Order\Cities Skylines II\Mods\ResidentCleaner`.

### Project structure

| File | Purpose |
| --- | --- |
| `Mod.cs` | Entry point, registers the options menu, translations and the cleaner system |
| `Setting.cs` | Options menu buttons and English / Traditional Chinese text |
| `ResidentCleanerSystem.cs` | Finds and deletes resident entities when a button is pressed |
| `Properties/PublishConfiguration.xml` | Paradox Mods page information |

### Disclaimer

Use at your own risk and always keep a backup of your save.

---

## 繁體中文

### 功能說明

有些存檔裡的市民（目前在建築物外活動的市民）會進入異常狀態，例如移除了車輛模組、停用了尋路或 AI 類模組，或是在乘客還在車上時刪除了公共交通車輛。遊戲更新這些市民時可能會直接閃退，而且 `Player.log` 裡不會留下有用的錯誤訊息。

ResidentCleaner 會在選項選單中加入兩個按鈕：

| 按鈕 | 刪除對象 |
| --- | --- |
| 清除異常市民 | 只刪除引用已不存在的車輛、車道或目的地的市民 |
| 清除所有外出市民 | 刪除所有目前在外活動的市民（第一個按鈕無法解決時再使用） |

被刪除的只是市民「外出中的分身」。市民本身、家庭和住處都會保留，之後會正常重新出門。

### 使用方法

1. 先備份存檔。
2. 在遊戲設定中開啟「**開始時暫停**」，然後載入存檔。
3. 按 Esc，進入「**選項**」，打開 **ResidentCleaner**，按下其中一個按鈕。
4. 另存新檔，然後開始模擬，確認城市可以正常運作。

清理結果會寫在 `Logs/ResidentCleaner.log`，例如：

```
ResidentCleaner: checked 60147 residents, deleted 94 (deleteAll = False)
```

不需要一直啟用這個模組，需要修復存檔時再開啟即可。

### 從原始碼編譯

需要：

- 已安裝官方模組開發工具鏈的天際線 2（遊戲內：選項 → 模組）
- .NET SDK

除了範本預設的引用之外，`ResidentCleaner.csproj` 還需要引用兩個遊戲組件：`Colossal.Mathematics` 和 `Colossal.Localization`。

工具鏈的後處理程式需要 .NET 6 執行環境。如果電腦只有較新的版本，編譯前先允許向上相容：

```powershell
$env:DOTNET_ROLL_FORWARD="Major"
dotnet build
```

編譯完成的模組會複製到 `%USERPROFILE%\AppData\LocalLow\Colossal Order\Cities Skylines II\Mods\ResidentCleaner`。

### 專案結構

| 檔案 | 用途 |
| --- | --- |
| `Mod.cs` | 進入點，註冊選項選單、翻譯和清理系統 |
| `Setting.cs` | 選項選單按鈕與英文、繁體中文文字 |
| `ResidentCleanerSystem.cs` | 按下按鈕時找出並刪除市民 |
| `Properties/PublishConfiguration.xml` | Paradox Mods 頁面資訊 |

### 免責聲明

使用風險請自行承擔，並務必保留存檔備份。
