using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Mono;
using HarmonyLib;
using Naninovel;
using Naninovel.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace SummerCloverPlugin
{
    [BepInPlugin("SummerCloverPlugin", "夏色四葉草作弊選單 Made By Yuki.kaco", "1.0.0")]
    public class Plugin :BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> ScriptMenuKey;
        private ConfigEntry<KeyCode> CustomVariableKey;
        private ConfigEntry<KeyCode> DebugGUIKey;

        private string toastMessage = "55555555555";
        private float toastDuration = 2f; // Duration to show the toast
        private float toastTimer = 0f;

        private Dictionary<string, string> previousLocalVariableMap = new Dictionary<string, string>();


        private void Awake()
        {
            Debug.Log("夏色四葉草插件 Made By Yuki.kaco");
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

            if (Input.GetKeyDown(KeyCode.Insert))
            {
                SaveLocalVariableMap();
            }

            if (Input.GetKeyDown(KeyCode.Home))
            {
                FindChangesInLocalVariableMap();
            }

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                CompareAndToastChanges();
            }

            
            if (Input.GetKeyDown(KeyCode.F6))
            {
                ShowToast(($"1234567890123456789012345678901234567890"));
            }

            if (toastTimer > 0)
            {
                toastTimer -= Time.deltaTime;
            }
            else
            {
                // Reset the toast message after the timer runs out
                toastMessage = string.Empty;
            }
        }

        private void CompareAndToastChanges()
        {
            // Attempt to get the instance of CustomVariableGUI
            var instance = Traverse.Create(typeof(CustomVariableGUI)).Field("instance").GetValue<CustomVariableGUI>();
            if (instance == null)
            {
                Debug.LogWarning("CustomVariableGUI instance is null.");
                return;
            }

            // Attempt to get the variableManager field
            var CustomVariableManager = Traverse.Create(instance).Field("variableManager").GetValue<ICustomVariableManager>();
            if (CustomVariableManager == null)
            {
                Debug.LogWarning("CustomVariableManager is null.");
                return;
            }

            // Use GetAllVariables to get the variables
            IReadOnlyCollection<CustomVariable> allVariables = CustomVariableManager.GetAllVariables();
            if (allVariables == null)
            {
                Debug.LogWarning("All variables collection is null.");
                return;
            }

            // Define the file path
            string filePath = Path.Combine(Paths.ConfigPath, "LocalVariableMap.txt");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("LocalVariableMap.txt does not exist. Please save the map first using F4.");
                return;
            }

            // Read the saved LocalVariableMap from the file
            var savedLocalVariableMap = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    savedLocalVariableMap[parts[0].Trim()] = parts[1].Trim();
                }
            }

            // Regular expression pattern to match "xxxInfo{num}" (e.g., KasumiInfo1, AyanoInfo2)
            string pattern = @"\w+Info\d";

            // Compare savedLocalVariableMap with the current variables
            StringBuilder changesBuilder = new StringBuilder();
            changesBuilder.AppendLine("Changes in LocalVariableMap:");

            ICustomVariableManager service = Engine.GetService<ICustomVariableManager>();

            foreach (var variable in allVariables)
            {
                // Skip variables that match the pattern "xxxInfo{num}"
                if (Regex.IsMatch(variable.Name, pattern))
                {
                    continue; // Skip these variables
                }

                if (!savedLocalVariableMap.TryGetValue(variable.Name, out var oldValue) || variable.Value != oldValue)
                {
                    ShowToast($"{variable.Name} changed from {oldValue} to {variable.Value}");
                    changesBuilder.AppendLine($"{variable.Name} changed from {oldValue} to {variable.Value}");
                    service.SetVariableValue(variable.Name, oldValue);
                }
            }
        }

        private void SaveLocalVariableMap()
        {
            // Attempt to get the instance of CustomVariableGUI
            var instance = Traverse.Create(typeof(CustomVariableGUI)).Field("instance").GetValue<CustomVariableGUI>();
            if (instance == null)
            {
                Debug.LogWarning("CustomVariableGUI instance is null.");
                return;
            }

            // Attempt to get the variableManager field
            var CustomVariableManager = Traverse.Create(instance).Field("variableManager").GetValue<ICustomVariableManager>();
            if (CustomVariableManager == null)
            {
                Debug.LogWarning("CustomVariableManager is null.");
                return;
            }

            // Use GetAllVariables to get the variables
            IReadOnlyCollection<CustomVariable> allVariables = CustomVariableManager.GetAllVariables();
            if (allVariables == null)
            {
                Debug.LogWarning("All variables collection is null.");
                return;
            }

            // Define the file path
            string filePath = Path.Combine(Paths.ConfigPath, "LocalVariableMap.txt");

            // Use a StringBuilder for efficient concatenation
            var logBuilder = new StringBuilder();

            // Save the current state of the variables
            previousLocalVariableMap.Clear();
            foreach (var variable in allVariables)
            {
                previousLocalVariableMap[variable.Name] = variable.Value;
                logBuilder.AppendLine($"{variable.Name}: {variable.Value}");
            }

            // Write the log to a file
            File.WriteAllText(filePath, logBuilder.ToString());

            // Optionally log the file path to the console
            ShowToast($"LocalVariableMap saved to: {filePath}");
        }

        private void FindChangesInLocalVariableMap()
        {
            // Attempt to get the instance of CustomVariableGUI
            var instance = Traverse.Create(typeof(CustomVariableGUI)).Field("instance").GetValue<CustomVariableGUI>();
            if (instance == null)
            {
                Debug.LogWarning("CustomVariableGUI instance is null.");
                return;
            }

            // Attempt to get the variableManager field
            var CustomVariableManager = Traverse.Create(instance).Field("variableManager").GetValue<ICustomVariableManager>();
            if (CustomVariableManager == null)
            {
                Debug.LogWarning("CustomVariableManager is null.");
                return;
            }

            // Use GetAllVariables to get the variables
            IReadOnlyCollection<CustomVariable> allVariables = CustomVariableManager.GetAllVariables();
            if (allVariables == null)
            {
                Debug.LogWarning("All variables collection is null.");
                return;
            }

            // Define the file path
            string filePath = Path.Combine(Paths.ConfigPath, "LocalVariableMap.txt");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("LocalVariableMap.txt does not exist. Please save the map first using F4.");
                return;
            }

            // Read the saved LocalVariableMap from the file
            var savedLocalVariableMap = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    savedLocalVariableMap[parts[0].Trim()] = parts[1].Trim();
                }
            }

            // Regular expression pattern to match "xxxInfo{num}" (e.g., KasumiInfo1, AyanoInfo2)
            string pattern = @"\w+Info\d";

            // Compare savedLocalVariableMap with the current variables
            var changesBuilder = new StringBuilder();
            changesBuilder.AppendLine("Changes in LocalVariableMap:");

            foreach (var variable in allVariables)
            {
                // Skip variables that match the pattern "xxxInfo{num}"
                if (Regex.IsMatch(variable.Name, pattern))
                {
                    continue; // Skip these variables
                }

                if (!savedLocalVariableMap.TryGetValue(variable.Name, out var oldValue) || variable.Value != oldValue)
                {
                    ShowToast($"{variable.Name} changed from {oldValue} to {variable.Value}");
                    changesBuilder.AppendLine($"{variable.Name} changed from {oldValue} to {variable.Value}");
                }
            }

            // Define the changes file path
            string changesFilePath = Path.Combine(Paths.ConfigPath, "LocalVariableMap_Changes.txt");

            // Write the changes to a file
            File.WriteAllText(changesFilePath, changesBuilder.ToString());

            // Optionally log the file path to the console
            ShowToast($"Changes written to: {changesFilePath}");
        }


        private void ShowToast(string message)
        {
            toastMessage += '\n' + message;
            toastTimer = toastDuration;
        }

        private void OnGUI()
        {
            if (toastTimer > 0)
            {
                // Calculate dynamic font size based on screen width/height
                int fontSize = Mathf.Clamp(Screen.height / 20, 20, 30); // Adjust font size based on screen height

                // Create GUIStyle
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = fontSize, // Set dynamic font size
                    alignment = TextAnchor.LowerRight, // Align to bottom-right corner
                    normal = { textColor = Color.white } // White text color
                };

                // Define the Rect for the toast message
                
                Rect rect = new Rect(
                    0,0,Screen.width,Screen.height
                );

                // Display the toast message
                GUI.Label(rect, toastMessage, style);
            }
        }


        public void SetCustomVariableGUIRect()
        {
            // Access and modify the windowRect field in the CustomVariableGUI class
            var traverse = Traverse.Create(typeof(CustomVariableGUI)).Field("windowRect");
            if (traverse.GetValue() is Rect currentRect)
            {
                currentRect.width = 400f + (float)Screen.width * 0.15f;
                currentRect.height = (float)Screen.height * 0.85f;

                // Set the updated Rect back to the field
                traverse.SetValue(currentRect);
            }
        }
    }
}
