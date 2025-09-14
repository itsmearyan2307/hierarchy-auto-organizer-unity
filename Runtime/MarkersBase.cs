using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itsnotmearyan.HierarchyOraganizerTool
{
    public class MarkersBase : MonoBehaviour
    {

        [Header("Text/Field Colour")]
        [Tooltip("Change the marker colour")]
        public Color fieldColor = Color.gray;

        [Tooltip("Change the text colour")]
        public Color textColor = Color.white;

        [Space]

        [Header("Displayed label in the Hierarchy")]
        [Tooltip("Change the name of the Marker, not from the inspector")]
        public string markerLabel = "New Marker";

        [Space]

        [Tooltip("Change the Text alignment settings of the marker")]
        [Header("Text Settings")]
        public FontStyle fontStyle = FontStyle.BoldAndItalic;
        public TextAnchor textAnchor = TextAnchor.MiddleLeft;

        [Space]

        [Header("Optional")]
        [Tooltip("Add an Icon to the Marker")]
        public Texture2D customIcon;





    }
}