# Summer Clover Cheat Menu

## Install / 安裝
- **Download** the zip file and extract it to the game's root folder (the folder containing `SummerClover.exe` and `UnityCrashHandler64.exe`).
- **下載** zip 文件並解壓縮到遊戲根目錄（會看到 `SummerClover.exe` 和 `UnityCrashHandler64.exe`）。

---

## Features / 功能
- **[F1] Variable Menu:** View/Modify game variables  
  **[F1] 變數選單：** 查看/修改遊戲變數  
- **[F2] Script Menu:** Go to any script  
  **[F2] 腳本選單：** 前往任意腳本  
- **[F3] DebugInfo Menu:** Check the current script name  
  **[F3] Debug 選單：** 查看當前腳本名稱  
- **[Insert]** Write the current variable to `BepInEx\config\LocalVariableMap.txt`  
  **[Insert]** 寫入當前變數到 `BepInEx\config\LocalVariableMap.txt`  
- **[Home]** Write changed variables to `BepInEx\config\LocalVariableMap_Changes.txt`  
  **[Home]** 寫入改變的變數到 `BepInEx\config\LocalVariableMap_Changes.txt`  
- **[PageUp]** Set `LocalVariableMap.txt` as the game's variable  
  **[PageUp]** 設定 `LocalVariableMap.txt` 當作遊戲的變數  

---
# Change HotKey / 修改熱鍵

- You can modify the hotkeys by editing the configuration file:  
  `BepInEx\config\SummerCloverPlugin.cfg`  
- 可以通過編輯設定檔來修改熱鍵：  
  `BepInEx\config\SummerCloverPlugin.cfg`

---

# Known Issue / 已知問題

- F2 Goto any script seem have some problem
but you can use for HCG

---

## Images / 圖片

### F1 Variable Menu
![F1 Variable Menu](/img/F1.png)

### F2 Script Menu
![F2 Script Menu](/img/F2.png)

### F3 DebugInfo Menu
![F3 DebugInfo Menu](/img/F3.png)

---

## Game Crash

If the game crashes, download the appropriate Unity libraries for your Unity version:

### Links
- Libraries:  
  https://unity.bepinex.dev/libraries/unity_version.zip
- Corlibs:  
  https://unity.bepinex.dev/corlibs/unity_version.zip

For example, for Unity version **2020.3.48**:  
- https://unity.bepinex.dev/libraries/2020.3.48.zip
- https://unity.bepinex.dev/corlibs/2020.3.48.zip
- ![Unity Version](/img/version.png)

### Instructions
- Extract the downloaded files.
- Place the contents into the `unstripped_corlib` folder.

![Unstripped Corlib Example](/img/corlib.png)

---

## Reference Stripped Assemblies
For additional guidance, refer to these resources:
- [BepInEx GitHub Issue #774 - Comment](https://github.com/BepInEx/BepInEx/issues/774#issuecomment-1937897640)
- [HackMD Reference for Stripped Assemblies](https://hackmd.io/@ghorsington/rJuLdZTzK)
