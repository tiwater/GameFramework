using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure.Levels.ObjectModel;
using UnityEditor;
using UnityEngine;

public class GameItemMetaHandler
{
    [MenuItem("Assets/Export GameItem Meta")]
    static void ExportGameItemMetas()
    {
        string path = "Assets/Game/RemoteAssets/Metas/Level/Level_desert.asset";
        Debug.Log(path);

        var level = AssetDatabase.LoadAssetAtPath<Level>(path);
        Debug.Log(level.name);
    }
}
