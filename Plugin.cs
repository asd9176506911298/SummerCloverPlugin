using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Mono;
using HarmonyLib;
using Naninovel;
using Naninovel.UI;
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
            CustomVariableKey = Config.Bind<KeyCode>("DebugMenu", "CustomVariableToggleKey", KeyCode.F1, "測試自訂變數開啟快捷鍵/CustomVariableToggleKey");
            ScriptMenuKey = Config.Bind<KeyCode>("DebugMenu", "ScriptMenuToggleKey", KeyCode.F2, "測試腳本介面開啟快捷鍵/ScriptMenuToggleKey");
            DebugGUIKey = Config.Bind<KeyCode>("DebugMenu", "DebugGUIToggleKey", KeyCode.F3, "測試介面開啟快捷鍵/DebugGUIToggleKey");
        }

        private void Update()
        {
            if (Input.GetKeyDown(ScriptMenuKey.Value))
            {
                ConsoleCommands.ToggleScriptNavigator();
            }

            if (Input.GetKeyDown(CustomVariableKey.Value))
            {
                SetCustomVariableGUIRect();
                ConsoleCommands.ToggleCustomVariableGUI();
            }

            if (Input.GetKeyDown(DebugGUIKey.Value))
            {
                ConsoleCommands.ToggleDebugInfoGUI();
            }
        }

        public void SetCustomVariableGUIRect()
        {
            // Access and modify the windowRect field in the CustomVariableGUI class
            var traverse = Traverse.Create(typeof(CustomVariableGUI)).Field("windowRect");
            if (traverse.GetValue() is Rect currentRect)
            {
                // Modify only the height
                currentRect.height = (float)Screen.height * 0.85f;

                // Set the updated Rect back to the field
                traverse.SetValue(currentRect);
            }
        }
    }
}
