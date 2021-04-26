using System;
using GameFramework.Debugging;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Variables.ObjectModel;
using GameFramework.Preferences;
using UnityEngine;

namespace AssemblyCSharp.Assets.GameFramework.Scripts.Preferences.PlayerDtoPrefsIntegration
{
    public class PlayerDtoPrefsHandler : IPreferences
    {
        public PlayerDtoPrefsHandler()
        {
        }


        /// <summary>
        /// Indicate that this implementaiton doesn't support secure prefs.
        /// </summary>
        public bool SupportsSecurePrefs
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flag indicating whether to use secure prefs.
        /// 
        /// Note: This property has no effect for standard Player Prefs.
        /// </summary>
        public bool UseSecurePrefs { get; set; }

        /// <summary>
        /// Flag indicating whether to migrate unsecure values automatically (only when UseSecurePrefs is set).
        /// 
        /// Note: This property has no effect for standard Player Prefs.
        /// </summary>
        public bool AutoConvertUnsecurePrefs { get; set; }

        private Variables storage
        {
            get
            {
                return GameManager.Instance.Players.Selected.PlayerDto.Props;
            }
        }

        /// <summary>
        /// The pass phrase that should be used. Be sure to override this with your own value.
        /// 
        /// Note: This property has no effect for standard Player Prefs.
        /// </summary>
        public string PassPhrase
        {
            set { }
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public void DeleteAll()
        {
            foreach(var variable in storage.BoolVariables)
            {
                variable.Value = variable.DefaultValue;
            }
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public void DeleteKey(string key, bool? useSecurePrefs = null)
        {
            PlayerPrefs.DeleteKey(key);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public float GetFloat(string key, float defaultValue = 0.0f, bool? useSecurePrefs = null)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public int GetInt(string key, int defaultValue = 0, bool? useSecurePrefs = null)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public string GetString(string key, string defaultValue = "", bool? useSecurePrefs = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        /// <summary>
        /// Get boolean preferences
        /// </summary>
        public bool GetBool(string key, bool defaultValue = false, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Boolean preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
            return defaultValue;
        }

        /// <summary>
        /// Get Vector2 preferences
        /// </summary>
        public Vector2? GetVector2(string key, Vector2? defaultValue = null, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Vector2 preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
            return defaultValue;
        }

        /// <summary>
        /// Get Vector3 preferences
        /// </summary>
        public Vector3? GetVector3(string key, Vector3? defaultValue = null, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Vector3 preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
            return defaultValue;
        }

        /// <summary>
        /// Get Color preferences
        /// </summary>
        public Color? GetColor(string key, Color? defaultValue = null, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Color preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
            return defaultValue;
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public bool HasKey(string key, bool? useSecurePrefs = null)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetFloat(string key, float value, bool? useSecurePrefs = null)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetInt(string key, int value, bool? useSecurePrefs = null)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetString(string key, string value, bool? useSecurePrefs = null)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// Set boolean preferences
        /// </summary>
        public void SetBool(string key, bool value, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("bool preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
        }

        /// <summary>
        /// Set Vector2 preferences
        /// </summary>
        public void SetVector2(string key, Vector2 value, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Vector2 preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
        }

        /// <summary>
        /// Set Vector3 preferences
        /// </summary>
        public void SetVector3(string key, Vector3 value, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Vector3 preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
        }

        /// <summary>
        /// Set Color preferences
        /// </summary>
        public void SetColor(string key, Color value, bool? useSecurePrefs = null)
        {
            MyDebug.LogWarning("Color preferences are only supported with the PlayerPrefs integration. See Main Menu | Window | Game Framework | Integrations Window for more details.");
        }
    }
}
