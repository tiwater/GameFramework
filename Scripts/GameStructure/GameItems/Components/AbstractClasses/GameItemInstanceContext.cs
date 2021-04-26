using System;
using GameFramework.GameStructure.GameItems.ObjectModel;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Represent the GameItems that each instance has different persistend status
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GameItemInstanceContext<T> : ShowPrefab<T> where T : GameItem
    {
        //The id for each instance
        public string ObjectId;
    }
}