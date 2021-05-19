using System.Linq;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Variables.ObjectModel;
using GameFramework.Preferences;
using GameFramework.Service;
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
                return true;
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
                if (GameManager.Instance.Players == null || GameManager.Instance.Players.Selected == null)
                {
                    if (_storage == null)
                    {
                        _storage = new Variables();
                    }
                    HasOfflineConfig = true;
                    return _storage;
                }
                else
                {
                    if (HasOfflineConfig)
                    {
                        //TODO: Sync Offline data to the player instance
                    }
                    return GameManager.Instance.Players.Selected.PlayerGameItem.ExtraProps;
                }
            }
        }

        private Variables _storage = null;

        private bool HasOfflineConfig = false;

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
            storage.BoolVariables.Clear();
            storage.ColorVariables.Clear();
            storage.FloatVariables.Clear();
            storage.IntVariables.Clear();
            storage.StringVariables.Clear();
            storage.Vector2Variables.Clear();
            storage.Vector3Variables.Clear();
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public void DeleteKey(string key, bool? useSecurePrefs = null)
        {
            storage.BoolVariables.RemoveAll(item => item.Tag == key);
            storage.ColorVariables.RemoveAll(item => item.Tag == key);
            storage.FloatVariables.RemoveAll(item => item.Tag == key);
            storage.IntVariables.RemoveAll(item => item.Tag == key);
            storage.Vector2Variables.RemoveAll(item => item.Tag == key);
            storage.Vector3Variables.RemoveAll(item => item.Tag == key);
            storage.StringVariables.RemoveAll(item => item.Tag == key);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public float GetFloat(string key, float defaultValue = 0.0f, bool? useSecurePrefs = null)
        {
            var variable = storage.GetFloat(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public int GetInt(string key, int defaultValue = 0, bool? useSecurePrefs = null)
        {
            var variable = storage.GetInt(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public string GetString(string key, string defaultValue = "", bool? useSecurePrefs = null)
        {
            var variable = storage.GetString(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Get boolean preferences
        /// </summary>
        public bool GetBool(string key, bool defaultValue = false, bool? useSecurePrefs = null)
        {
            var variable = storage.GetBool(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Get Vector2 preferences
        /// </summary>
        public Vector2? GetVector2(string key, Vector2? defaultValue = null, bool? useSecurePrefs = null)
        {
            var variable = storage.GetVector2(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Get Vector3 preferences
        /// </summary>
        public Vector3? GetVector3(string key, Vector3? defaultValue = null, bool? useSecurePrefs = null)
        {
            var variable = storage.GetVector3(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Get Color preferences
        /// </summary>
        public Color? GetColor(string key, Color? defaultValue = null, bool? useSecurePrefs = null)
        {
            var variable = storage.GetColor(key);
            return variable == null ? defaultValue : variable.Value;
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public bool HasKey(string key, bool? useSecurePrefs = null)
        {
            return (storage.BoolVariables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.ColorVariables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.FloatVariables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.IntVariables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.Vector2Variables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.Vector3Variables.Where(item => item.Tag == key).Count() > 0) ||
                (storage.StringVariables.Where(item => item.Tag == key).Count() > 0);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs.
        /// </summary>
        public void Save()
        {
            PlayerGameItemService.Instance.UpdatePlayer();
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetFloat(string key, float value, bool? useSecurePrefs = null)
        {
            storage.SetFloat(key, value);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetInt(string key, int value, bool? useSecurePrefs = null)
        {
            storage.SetInt(key, value);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs - note that useSecurePrefs is ignored.
        /// </summary>
        public void SetString(string key, string value, bool? useSecurePrefs = null)
        {
            storage.SetString(key, value);
        }

        /// <summary>
        /// Set boolean preferences
        /// </summary>
        public void SetBool(string key, bool value, bool? useSecurePrefs = null)
        {
            storage.SetBool(key, value);
        }

        /// <summary>
        /// Set Vector2 preferences
        /// </summary>
        public void SetVector2(string key, Vector2 value, bool? useSecurePrefs = null)
        {

            storage.SetVector2(key, value);
        }

        /// <summary>
        /// Set Vector3 preferences
        /// </summary>
        public void SetVector3(string key, Vector3 value, bool? useSecurePrefs = null)
        {

            storage.SetVector3(key, value);
        }

        /// <summary>
        /// Set Color preferences
        /// </summary>
        public void SetColor(string key, Color value, bool? useSecurePrefs = null)
        {

            storage.SetColor(key, value);
        }
    }
}
