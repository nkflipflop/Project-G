using Pooling;
using UnityEditor;
using UnityEngine;

namespace LootSystem.Editor
{
	[CustomPropertyDrawer(typeof(PoolObjectItem))]
	public class PoolObjectItemDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = 16;
	        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, toggleOnLabelClick: true);

	        if (property.isExpanded)
	        {
		        EditorGUI.BeginProperty(position, label, property);
		        
		        SerializedProperty type = property.FindPropertyRelative("type");
		        SerializedProperty consumable = property.FindPropertyRelative("consumable");

		        Rect typePos = new Rect(position.x,
			        position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
			        position.width, EditorGUIUtility.singleLineHeight);
		        
		        int cachedIndent = EditorGUI.indentLevel;
		        EditorGUI.indentLevel++;

		        EditorGUI.PropertyField(typePos, type, new GUIContent("Type"));
		        if (type.enumValueFlag == (int)ObjectType.ConsumableObject)
		        {
			        Rect consumablePos = new Rect(position.x,
				        typePos.y + EditorGUIUtility.standardVerticalSpacing +
				        EditorGUI.GetPropertyHeight(consumable), position.width, EditorGUIUtility.singleLineHeight);
			        EditorGUI.PropertyField(consumablePos, consumable, new GUIContent("Consumable"));
		        }
		        EditorGUI.indentLevel = cachedIndent;
		        EditorGUI.EndProperty();
	        }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
	        if (!property.isExpanded)
	        {
		        return EditorGUIUtility.singleLineHeight;
	        }
	        
	        SerializedProperty type = property.FindPropertyRelative("type");
	        SerializedProperty consumable = property.FindPropertyRelative("consumable");
	        float height = EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(type) +
	                       EditorGUIUtility.standardVerticalSpacing;
	        if (type.enumValueFlag == (int)ObjectType.ConsumableObject)
	        {
		        height += EditorGUI.GetPropertyHeight(consumable) + EditorGUIUtility.standardVerticalSpacing;
	        }
	        return height;
        }
	}
}