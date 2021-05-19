
using GameFramework.GameStructure.GameItems.Editor.AbstractClasses;
using UnityEditor;

namespace GameFramework.GameStructure.Characters.Editor
{
    [CustomEditor(typeof(CharacterHolder))]
    public class CharacterHolderEditor : ShowPrefabEditor
    {
        SerializedProperty _equipmentInfosProperty;

        public new void OnEnable()
        {
            base.OnEnable();
            _equipmentInfosProperty = serializedObject.FindProperty("EquipmentInfos");
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(_equipmentInfosProperty);
        }
    }
}