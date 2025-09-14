using UnityEditor;
using UnityEngine;


namespace itsnotmearyan.HierarchyOraganizerTool
{
    [InitializeOnLoad]
    public static class MarkerLabelColorizer
    {
        private static readonly Vector2 offset = new Vector2(20, 1);

        static MarkerLabelColorizer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;


            MarkersBase marker = go.GetComponent<MarkersBase>();
            if (marker == null) return;
            go.name = marker.markerLabel;


            EditorGUI.DrawRect(selectionRect, marker.fieldColor);


            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                alignment = marker.textAnchor,
                fontSize = 13,
                fontStyle = marker.fontStyle,
                normal = new GUIStyleState { textColor = marker.textColor }
            };
            if (marker.customIcon != null)
            {
                Rect iconRect = new Rect(selectionRect.x + 2, selectionRect.y + 1, 14, 14);
                GUI.DrawTexture(iconRect, marker.customIcon, ScaleMode.ScaleToFit);
            }


            Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
            EditorGUI.LabelField(offsetRect, go.name, style);
        }
    }
}