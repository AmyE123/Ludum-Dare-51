using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MenuScreen.ElementItem))]
public class MSElementItemEditor : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var elementRect = new Rect(position.x, position.y, position.width - 54, position.height);
        var delayRect = new Rect(position.x + position.width - 50, position.y, 50, position.height);

        SerializedProperty elementProp = property.FindPropertyRelative("element");
        SerializedProperty delayProp = property.FindPropertyRelative("delay");

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(elementRect, elementProp, GUIContent.none);
        EditorGUI.PropertyField(delayRect, delayProp, GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}