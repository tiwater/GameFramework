//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using GameFramework.GameStructure.GameItems.Editor;
using UnityEditor;
using UnityEngine;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using static GameFramework.GameStructure.Model.AddressableGameItemMeta;

namespace GameFramework.GameStructure.AddressableGameItems.Editor
{
    [CustomEditor(typeof(AddressableGameItem))]
    public class AddressableGameItemEditor : GameItemEditor
    {
        SerializedProperty _giContentTypeProperty;
        SerializedProperty _giAddressableNameProperty;
        SerializedProperty _giAddressableLabelProperty;
        SerializedProperty _giThumbnailProperty;
        SerializedProperty _giSlotsProperty;
        SerializedProperty _giSupportItemProperty;

        protected override void OnEnable()
        {
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            base.OnEnable();
            _giContentTypeProperty = serializedObject.FindProperty("_contentType");
            _giAddressableNameProperty = serializedObject.FindProperty("AddressableName");
            _giAddressableLabelProperty = serializedObject.FindProperty("AddressableLabel");
            _giThumbnailProperty = serializedObject.FindProperty("ThumbnailUrl");
            _giSlotsProperty = serializedObject.FindProperty("SupportedSlots");
            _giSupportItemProperty = serializedObject.FindProperty("_supportItems");
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Addressable GameItem", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use these settings to provide customisation for AddressableGameItems.\n\nFor automatic loading instances should be in a folder 'Resources\\AddressableGameItem' and named 'AddressableGameItem_<number>'\n\nYou can create your own AddressableGameItem derived classes to hold custom properties and / or code", MessageType.Info);


            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_giAddressableNameProperty, new GUIContent("Addressable Name"));
            EditorGUILayout.PropertyField(_giAddressableLabelProperty, new GUIContent("Addressable Label"));
            EditorGUILayout.PropertyField(_giContentTypeProperty, new GUIContent("Content Type"));
            EditorGUILayout.PropertyField(_giThumbnailProperty, new GUIContent("Thumbnail URL"));
            EditorGUILayout.PropertyField(_giSlotsProperty, new GUIContent("Supported Slots"));
            EditorGUILayout.PropertyField(_giSupportItemProperty, new GUIContent("Supported Item"));

            EditorGUILayout.EndVertical();

            DrawProperties();
        }

        protected new void DrawProperties()
        {
            DrawBasicProperties();
            if (_giContentTypeProperty.enumValueIndex != (int)ContentType.Skin)
            {
                DrawPrefabs();
            }
            DrawSprites();
            if (_giContentTypeProperty.enumValueIndex == (int)ContentType.Skin)
            {
                DrawTextures();
            }
            DrawVariables();
        }
    }
}