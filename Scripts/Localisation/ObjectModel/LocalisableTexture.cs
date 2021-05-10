using System;
using GameFramework.Localisation.ObjectModel.AbstractClasses;
using UnityEngine;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// Class to hold information about localisable prefabs.
    /// </summary>
    [Serializable]
    public class LocalisableTexture : LocalisableObject
    {

        /// <summary>
        /// The default prefab that should be used.
        /// </summary>
        public new Texture Default
        {
            get { return base.Default as Texture; }
            set { base.Default = value; }
        }


        /// <summary>
        /// Get a prefab that corresponds to the currently set language
        /// </summary>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Texture GetTexture(bool fallbackToDefault = true)
        {
            return GetObject(fallbackToDefault) as Texture;
        }


        /// <summary>
        /// Get a prefab that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Texture GetTexture(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as Texture;
        }


        /// <summary>
        /// Get a prefab that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Texture GetTexture(string language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as Texture;
        }
    }
}
