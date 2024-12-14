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

namespace SummerCloverPlugin
{
    [BepInPlugin("SummerCloverPlugin", "Summer Clover Cheat Menu Made By Yuki.kaco", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> ScriptMenuKey;
        private ConfigEntry<KeyCode> CustomVariableKey;
        private ConfigEntry<KeyCode> DebugGUIKey;
        private ConfigEntry<KeyCode> SaveLocalVariableKey;
        private ConfigEntry<KeyCode> FindChangesKey;
        private ConfigEntry<KeyCode> CompareAndModifyKey;

        private string toastMessage = "55555555555";
        private float toastDuration = 2f; // Duration to show the toast
        private float toastTimer = 0f;

        private Dictionary<string, string> previousLocalVariableMap = new Dictionary<string, string>();

        private ICustomVariableManager customVariableManager;

        private void Awake()
        {
            Debug.Log("Summer Clover Cheat Menu Made By Yuki.kaco");
            CustomVariableKey = Config.Bind<KeyCode>("DebugMenu", "CustomVariableToggleKey", KeyCode.F1, "測試自訂變數開啟快捷鍵 / CustomVariableToggleKey");
            ScriptMenuKey = Config.Bind<KeyCode>("DebugMenu", "ScriptMenuToggleKey", KeyCode.F2, "測試腳本介面開啟快捷鍵 / ScriptMenuToggleKey");
            DebugGUIKey = Config.Bind<KeyCode>("DebugMenu", "DebugGUIToggleKey", KeyCode.F3, "測試介面開啟快捷鍵 / DebugGUIToggleKey");
            SaveLocalVariableKey = Config.Bind<KeyCode>("DebugMenu", "SaveLocalVariableKey", KeyCode.Insert, "儲存當前變數快捷鍵 / SaveLocalVariableHotKey");
            FindChangesKey = Config.Bind<KeyCode>("DebugMenu", "FindChangesKey", KeyCode.Home, "比較變數變化快捷鍵 / CmpVariableChangeHotKey");
            CompareAndModifyKey = Config.Bind<KeyCode>("DebugMenu", "CompareAndModifyKey", KeyCode.PageUp, "修改變數快捷鍵 / ModifyVariableHotKey");

            // Initialize ICustomVariableManager here to avoid repetitive calls
            InitializeCustomVariableManager();
        }

        private void InitializeCustomVariableManager()
        {
            if (customVariableManager == null)
            {
                customVariableManager = Engine.GetService<ICustomVariableManager>();
                if (customVariableManager == null)
                {
                    Debug.LogWarning("ICustomVariableManager service is still null after initialization.");
                }
            }
        }

        private void Update()
        {
            InitializeCustomVariableManager();  // Ensure customVariableManager is initialized on each update

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

            if (Input.GetKeyDown(SaveLocalVariableKey.Value))
            {
                SaveLocalVariableMap();
            }

            if (Input.GetKeyDown(FindChangesKey.Value))
            {
                FindChangesInLocalVariableMap();
            }

            if (Input.GetKeyDown(CompareAndModifyKey.Value))
            {
                CompareAndModify();
            }

#if DEBUG
            if (Input.GetKeyDown(KeyCode.F6))
            {
                ShowToast("1234567890123456789012345678901234567890");
            }
#endif

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

        private void CompareAndModify()
        {
            InitializeCustomVariableManager();  // Ensure customVariableManager is initialized before usage

            // Check if customVariableManager is still null
            if (customVariableManager == null)
            {
                Debug.LogWarning("ICustomVariableManager is not initialized.");
                return;
            }

            IReadOnlyCollection<CustomVariable> allVariables = customVariableManager.GetAllVariables();
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
                    if(!oldValue.IsNullOrWhiteSpace())
                        customVariableManager.SetVariableValue(variable.Name, oldValue);
                }
            }
        }

        private void SaveLocalVariableMap()
        {
            InitializeCustomVariableManager();  // Ensure customVariableManager is initialized before usage

            // Check if customVariableManager is null
            if (customVariableManager == null)
            {
                Debug.LogWarning("ICustomVariableManager is not initialized.");
                return;
            }

            IReadOnlyCollection<CustomVariable> allVariables = customVariableManager.GetAllVariables();
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
            //ShowToast($"LocalVariableMap saved to: \n{filePath}");
            ShowToast($"LocalVariableMap saved");
        }

        private void FindChangesInLocalVariableMap()
        {
            InitializeCustomVariableManager();  // Ensure customVariableManager is initialized before usage

            // Check if customVariableManager is null
            if (customVariableManager == null)
            {
                Debug.LogWarning("ICustomVariableManager is not initialized.");
                return;
            }

            IReadOnlyCollection<CustomVariable> allVariables = customVariableManager.GetAllVariables();
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
            //ShowToast($"Changes written to: \n{changesFilePath}");
            ShowToast($"Changes written");
        }

        private void ShowToast(string message)
        {
            // Limit the toast message length to avoid excessive length
            const int maxMessageLength = 5000; // Adjust based on your needs
            toastMessage += '\n' + message;

            if (toastMessage.Length > maxMessageLength)
            {
                toastMessage = toastMessage.Substring(0, maxMessageLength);  // Truncate if necessary
            }

            toastTimer = toastDuration;
        }


        private void OnGUI()
        {
            if (toastTimer > 0)
            {
                // Adjust the font size range (max font size set to 20 for smaller text)
                int fontSize = Mathf.Clamp(Screen.height / 25, 15, 20); // Smaller range for font size
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = fontSize,
                    alignment = TextAnchor.LowerRight,
                    normal = { textColor = Color.white },
                    wordWrap = true
                };

                GUIStyle backgroundStyle = new GUIStyle()
                {
                    normal = { background = MakeTexture(2, 2, new Color(0f, 0f, 0f, 0.3f)) }
                };

                // Position the toast message box at the bottom right
                Rect rect = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height - 40); // Adjust positioning and size
                GUI.Box(rect, GUIContent.none, backgroundStyle);
                GUI.Label(rect, toastMessage, style);
            }
        }

        // Helper function to create a texture (for background)
        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
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
