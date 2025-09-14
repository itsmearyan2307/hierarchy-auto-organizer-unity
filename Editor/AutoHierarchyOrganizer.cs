using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace itsnotmearyan.HierarchyOraganizerTool
{

    public enum GroupByType
    {
        Name,
        Tag
    }

    public class AutoHierarchyOrganizer : EditorWindow
    {
        private bool hasArrangedEnvironment = false;

        private bool showGroupingOptions = false;
        private GroupByType groupBy = GroupByType.Name;
        private List<string> excludeTags = new List<string>();
        private string newExcludeTag = "";

        [MenuItem("Tools/Hierarchy Organizer")]
        public static void ShowWindow()
        {
            var window = GetWindow<AutoHierarchyOrganizer>("Auto Hierarchy Organizer");
            window.minSize = new Vector2(400, 420);
            window.maxSize = new Vector2(500, 900);
        }
        private void OnGUI()
        {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };

            GUIStyle sectionHeader = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                padding = new RectOffset(12, 12, 7, 7)
            };

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13,
                fixedHeight = 32,
                fontStyle = FontStyle.Bold,
                
              

            };
            GUILayout.Space(10);
            GUILayout.Label("Hierarchy Auto Organizer", titleStyle);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width(320));
            // ---------- Quick Tool Section ----------
            GUILayout.BeginVertical("box");
            GUILayout.Label("Quick Tool", sectionHeader);
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Generate Empty Marker", "Create an Empty Marker"), buttonStyle))
            {
                GameObject newMarker = new GameObject("newMarker");
                newMarker.AddComponent<MarkersBase>();
                Selection.activeGameObject = newMarker;
                Undo.RegisterCreatedObjectUndo(newMarker, "Create Marker");
            }
            GUILayout.EndVertical();
            GUILayout.Space(20);
            GUILayout.BeginVertical("box");
            GUILayout.Label("Auto Organizers", sectionHeader);

            GUILayout.Space(10);

            if (GUILayout.Button(new GUIContent("Remove Unuseful GameObjects", "Deletes empty or unused GameObjects in the scene."), buttonStyle))
            {
                EmptyGameObjectsOrganizer.PrepareEmptyGameObjects();
            }
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Auto Arrange Useful Empty GameObjects", "Groups referenced empty GameObjects under manager/transform parents."), buttonStyle))
            {
                EmptyGameObjectsOrganizer.ArrangeEmptyGameObjects();
            }
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Auto Arrange UI Elements", "Groups UI elements under a central UIRoot."), buttonStyle))
            {
                UIOrganizer.AutoArrangeUIElements();
            }
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Auto Arrange 3D Environment", "Moves all 3D environment objects into a root container."), buttonStyle))
            {
                EnvironmentOrganizer.PrepareEnvironmentObjects(out _, out _, excludeTags);
                hasArrangedEnvironment = true;
            }
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Auto Arrange Environment Children", "Groups environment objects by name or tag into subfolders."), buttonStyle))
            {
                EnvironmentOrganizer.AutoArrangeEnvironmentHierarchy(groupBy, excludeTags);
                hasArrangedEnvironment = false;
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");
            showGroupingOptions = EditorGUILayout.Foldout(showGroupingOptions, " Environment Grouping Options", true, EditorStyles.foldoutHeader);
            if (showGroupingOptions)
            {
                GUILayout.Space(5);
                groupBy = (GroupByType)EditorGUILayout.EnumPopup("Group By", groupBy);

                GUILayout.Label("Exclude GameObjects With Tags:");
                for (int i = 0; i < excludeTags.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    excludeTags[i] = GUILayout.TextField(excludeTags[i]);
                    if (GUILayout.Button("X", GUILayout.Width(40)))
                    {
                        excludeTags.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                newExcludeTag = GUILayout.TextField(newExcludeTag);
                if (GUILayout.Button("Add", GUILayout.Width(40)) && !string.IsNullOrWhiteSpace(newExcludeTag) && !excludeTags.Contains(newExcludeTag))
                {
                    excludeTags.Add(newExcludeTag);
                    newExcludeTag = "";
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical(); // End main content vertical
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // End main horizontal layout
            GUILayout.Space(10);
        }

    }
}


