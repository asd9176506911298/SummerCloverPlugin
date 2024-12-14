# Summer Clover Cheat Menu

## Install / 安裝
- **Download** the zip file and extract it to the game's root folder (where `SummerClover.exe` and `UnityCrashHandler64.exe` are located).  
- **下載** zip 文件，並解壓縮到遊戲根目錄（包含 `SummerClover.exe` 和 `UnityCrashHandler64.exe` 的文件夾）。  
![bepinex](/img/bepinex.png)  
---

## Features / 功能
- **[F1] Variable Menu:** View and modify game variables.  
  **[F1] 變數選單：** 查看並修改遊戲變數。  
- **[F2] Script Menu:** Jump to any script.  
  **[F2] 腳本選單：** 前往任意腳本。  
- **[F3] Debug Info Menu:** Check the current script name.  
  **[F3] Debug 選單：** 查看當前腳本名稱。  
- **[Insert]:** Save current variables to `BepInEx\config\LocalVariableMap.txt`.  
  **[Insert]：** 將當前變數保存到 `BepInEx\config\LocalVariableMap.txt`。  
- **[Home]:** Save modified variables to `BepInEx\config\LocalVariableMap_Changes.txt`.  
  **[Home]：** 將修改後的變數保存到 `BepInEx\config\LocalVariableMap_Changes.txt`。  
- **[PageUp]:** Load `LocalVariableMap.txt` as the game's variable set.  
  **[PageUp]：** 將 `LocalVariableMap.txt` 設為遊戲變數。  

---

## Recommended Variables to Modify / 建議修改的變數
- **PlayerEventAction:** Action (行動)  
- **PlayerAp:** Energy (體力)  
- **PlayerSex:** Desire (性慾)  
- **NewPressure:** Stress (壓力)  
- **PlayerMoney:** Money (金錢)  
- **PlayerSystem:** Points (點數)  

> **Note:** Modifying these variables is less likely to break the game or cause unexpected issues with your save file.  
> **注意：** 修改上述變數通常不會破壞遊戲或存檔，導致非預期的問題。  

---

## Change Hotkeys / 修改熱鍵
- You can customize hotkeys by editing the configuration file:  
  `BepInEx\config\SummerCloverPlugin.cfg`  
- 可以通過編輯設定檔來自定義熱鍵：  
  `BepInEx\config\SummerCloverPlugin.cfg`  

---

## Known Issues / 已知問題
- **[F2] Script Menu:** Jumping to certain scripts may cause issues, but it works for HCG scenes.  
  **[F2] 腳本選單：** 前往某些腳本可能會有問題，但適用於 HCG 場景。  

- **[PageUp]:** Using this feature may cause unexpected problems. Always back up your save files beforehand.  
  **[PageUp]：** 使用此功能可能導致未預期的問題，請務必提前備份存檔。  

---

## Game Crashes / 遊戲崩潰

If the game crashes, download the required Unity libraries for your Unity version:

### Links
- **Libraries:**  
  [https://unity.bepinex.dev/libraries/unity_version.zip](https://unity.bepinex.dev/libraries/unity_version.zip)  
- **Corlibs:**  
  [https://unity.bepinex.dev/corlibs/unity_version.zip](https://unity.bepinex.dev/corlibs/unity_version.zip)  
  ![game version](/img/version.png)  
For example, for Unity version **2020.3.48**:  
- [Libraries for 2020.3.48](https://unity.bepinex.dev/libraries/2020.3.48.zip)  
- [Corlibs for 2020.3.48](https://unity.bepinex.dev/corlibs/2020.3.48.zip)  

---

### Instructions of Game Crashes/ 安裝說明 遊戲崩潰
1. Extract the downloaded files.  
2. Place the contents into the `unstripped_corlib` folder in your game directory.  
![corlib](/img/corlib.png)  
---

## Reference for Stripped Assemblies
For more information, refer to these resources:  
- [BepInEx GitHub Issue #774 - Comment](https://github.com/BepInEx/BepInEx/issues/774#issuecomment-1937897640)  
- [HackMD Guide for Stripped Assemblies](https://hackmd.io/@ghorsington/rJuLdZTzK)  

---

## Images / 圖片

### F1 Variable Menu  
![F1 Variable Menu](/img/F1.png)  

### F2 Script Menu  
![F2 Script Menu](/img/F2.png)  

### F3 Debug Info Menu  
![F3 Debug Info Menu](/img/F3.png)  
