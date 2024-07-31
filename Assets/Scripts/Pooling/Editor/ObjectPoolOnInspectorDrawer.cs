using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;

namespace Pooling.Editor
{
    [CustomPropertyDrawer(typeof(ObjectPoolOnInspector))]
    public class ObjectPoolOnInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ObjectPoolOnInspector pool = (ObjectPoolOnInspector)PropertyUtility.GetTargetObjectOfProperty(property);
            pool?.Refresh();
            
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SerializedProperty activeObjects = property.FindPropertyRelative("activeObjects");
            SerializedProperty inActiveObjects = property.FindPropertyRelative("inActiveObjects");

            Rect inActiveObjectsPos = new Rect(position.x,
                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);
            Rect activeObjectsPos = new Rect(position.x,
                inActiveObjectsPos.y + EditorGUIUtility.standardVerticalSpacing +
                EditorGUI.GetPropertyHeight(inActiveObjects), position.width, EditorGUIUtility.singleLineHeight);

            int cachedIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(inActiveObjectsPos, inActiveObjects, new GUIContent("In-Active"));
            
            Color cachedColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            EditorGUI.PropertyField(activeObjectsPos, activeObjects, new GUIContent("Active"));
            GUI.backgroundColor = cachedColor;
            
            EditorGUI.indentLevel = cachedIndent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty passiveObjects = property.FindPropertyRelative("inActiveObjects");
            SerializedProperty activeObjects = property.FindPropertyRelative("activeObjects");
            return EditorGUIUtility.singleLineHeight + (3 * EditorGUIUtility.standardVerticalSpacing) +
                   EditorGUI.GetPropertyHeight(passiveObjects) + EditorGUI.GetPropertyHeight(activeObjects);
        }
    }
}