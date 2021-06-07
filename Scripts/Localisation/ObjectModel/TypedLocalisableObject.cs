using System;
using GameFramework.Localisation.ObjectModel.AbstractClasses;
using UnityEngine;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// Class to hold information about localisable assets.
    /// </summary>
    [Serializable]
    public class TypedLocalisableObject<T> : LocalisableObject where T : UnityEngine.Object
    {

        /// <summary>
        /// The default asset that should be used.
        /// </summary>
        public new T Default
        {
            get { return base.Default as T; }
            set { base.Default = value; }
        }


        /// <summary>
        /// Get an asset that corresponds to the currently set language
        /// </summary>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public T GetAsset(bool fallbackToDefault = true)
        {
            return GetObject(fallbackToDefault) as T;
        }


        /// <summary>
        /// Get an asset that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public T GetAsset(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as T;
        }


        /// <summary>
        /// Get an asset that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public T GetAsset(string language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as T;
        }
    }
}
