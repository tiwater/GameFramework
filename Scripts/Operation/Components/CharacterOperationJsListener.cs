using System;
using GameFramework.GameStructure.Characters;
using GameFramework.GameStructure.Characters.ObjectModel;
using UnityEngine;

namespace GameFramework.Operation.Components
{
    /// <summary>
    /// Listen to OperationMessage and forward to the JsBehaviour for the CharacterHolder
    /// </summary>
    [RequireComponent(typeof(CharacterHolder))]
    public class CharacterOperationJsListener : OperationJsListener<CharacterHolder, Character>
    {
    }
}