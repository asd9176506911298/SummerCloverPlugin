using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Mono;
using Naninovel;
using UnityEngine;

namespace SummerCloverPlugin
{
    [BepInPlugin("SummerCloverPlugin", "夏色四葉草插件", "1.0.0")]
    public class Plugin :BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> ScriptMenuKey;
        private ConfigEntry<KeyCode> CustomVariableKey;
        private ConfigEntry<KeyCode> DebugGUIKey;

        private void Awake()
        {
            Debug.Log("夏色四葉草插件");
            ScriptMenuKey = Config.Bind<KeyCode>("DebugMenu", "ScriptMenuToggleKey", KeyCode.F1, "測試腳本介面開啟快捷鍵");
            CustomVariableKey = Config.Bind<KeyCode>("DebugMenu", "CustomVariableToggleKey", KeyCode.F2, "測試自訂變數開啟快捷鍵");
            DebugGUIKey = Config.Bind<KeyCode>("DebugMenu", "DebugGUIToggleKey", KeyCode.F3, "測試介面開啟快捷鍵");

        }

        private void Update()
        {
            if (Input.GetKeyDown(ScriptMenuKey.Value))
            {
                ConsoleCommands.ToggleScriptNavigator();
            }

            if (Input.GetKeyDown(CustomVariableKey.Value))
            {
                ConsoleCommands.ToggleCustomVariableGUI();
            }

            if (Input.GetKeyDown(DebugGUIKey.Value))
            {
                ConsoleCommands.ToggleDebugInfoGUI();
            }
        }
    }
}
