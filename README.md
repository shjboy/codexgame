# codexgame

此資料庫包含一個 Unity 2D 遊戲專案的基本結構。

## 資料夾結構
- `Assets/` - 遊戲資源與腳本
- `Packages/` - 套件資訊
- `ProjectSettings/` - Unity 專案設定

## 棒球轉盤遊戲
`Assets/Scripts/BaseballWheelGame.cs` 提供簡易的轉盤機制來模擬棒球規則。將此腳本與顯示結果、局數與分數的 UI 元件及旋轉按鈕一併掛載到 GameObject。按下按鈕後會隨機出現十種結果，例如三振、四壞球或安打等，並自動追蹤出局數、上下半局與兩隊得分。
