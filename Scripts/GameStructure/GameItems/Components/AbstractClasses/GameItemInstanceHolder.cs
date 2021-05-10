using UnityEngine;
using System.Collections;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;

public abstract class GameItemInstanceHolder<T> : ShowPrefab<T> where T : GameItem
{

    /// <summary>
    /// The PlayerGameItem this context represents.
    /// </summary>
    public PlayerGameItem PlayerGameItem;
}
