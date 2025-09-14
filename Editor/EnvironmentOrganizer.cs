using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace itsnotmearyan.HierarchyOraganizerTool
{

    public static class EnvironmentOrganizer
    {
        private static EnvironmentRootMarker envrootMarker;
        private static List<GameObject> environmentObjects = new();

        public static void PrepareEnvironmentObjects(out GameObject root, out List<GameObject> collected, List<string> excludeTags)
        {
            GameObject[] envGameObj = Object.FindObjectsOfType<GameObject>();
            envrootMarker = Object.FindFirstObjectByType<EnvironmentRootMarker>();

            if (envrootMarker == null)
            {
                GameObject environmentRootObj = new GameObject("-------ENVIRONMENT-------");
                envrootMarker = environmentRootObj.AddComponent<EnvironmentRootMarker>();
                Debug.Log("Created ENVIRONMENT root.");
            }

            environmentObjects.Clear();

            foreach (GameObject go in envGameObj)
            {
                if (go == null || !go.scene.IsValid() || go.transform.IsChildOf(envrootMarker.transform))
                    continue;
                if (excludeTags.Contains(go.tag))
                    continue;

                Transform targetRoot = GetRootWithRenderer(go);

                if (targetRoot != null && !environmentObjects.Contains(targetRoot.gameObject))
                {
                    Undo.SetTransformParent(targetRoot, envrootMarker.transform, "Group Environment");
                    Debug.Log($"Grouped in ENVIRONMENT: {targetRoot.name}");
                    environmentObjects.Add(targetRoot.gameObject);
                }
            }

            root = envrootMarker.gameObject;
            collected = environmentObjects;
        }

        public static void AutoArrangeEnvironmentHierarchy(GroupByType groupBy, List<string> excludeTags)
        {
            if (envrootMarker == null)
                envrootMarker = Object.FindFirstObjectByType<EnvironmentRootMarker>();

            environmentObjects.Clear();
            GameObject[] envGameObj = Object.FindObjectsOfType<GameObject>();


            if (envrootMarker != null)
            
            {
                foreach (Transform t in envrootMarker.transform)
                {

                    GameObject go = t.gameObject;
                    environmentObjects.Add(go);

                }

                Dictionary<string, List<GameObject>> groupedObjects = new();

                foreach (GameObject go in environmentObjects)
                {
                    string baseName = "";

                    if (groupBy == GroupByType.Name)
                        baseName = GetBaseName(go.name);
                    else if (groupBy == GroupByType.Tag)
                        baseName = GetBaseName(go.tag);

                    if (!groupedObjects.ContainsKey(baseName))
                        groupedObjects[baseName] = new List<GameObject>();

                    groupedObjects[baseName].Add(go);
                }

                foreach (var group in groupedObjects)
                {
                    if (group.Value.Count > 1)
                    {
                        GameObject groupParent = new GameObject(group.Key);
                        groupParent.transform.SetParent(envrootMarker.transform, false);

                        foreach (GameObject go in group.Value)
                        {

                            if (!group.Value.Exists(other => other != go && go.transform.IsChildOf(other.transform)))
                            {
                                Undo.SetTransformParent(go.transform, groupParent.transform, "Group Object");
                                Debug.Log($"Added to {group.Key}: {go.name}");
                            }
                        }
                    }
                    else
                    {
                        group.Value[0].transform.SetParent(envrootMarker.transform, false);
                    }
                }

                GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
                EmptyGameObjectsOrganizer.DeleteGameObjects(allObjects);

            }
           
        }

        private static string GetBaseName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsDigit(c) || c == '_' || c == '(')
                    return name.Substring(0, i).Trim();
            }

            return name.Trim();
        }



        private static Transform GetRootWithRenderer(GameObject go)
        {
            if (go == null) return null;

            Transform current = go.transform;

            
            while (current.parent != null && current.parent.GetComponent<Transform>() != null)
            {
                
                if (current.parent.GetComponent<MeshFilter>() || current.parent.GetComponent<MeshRenderer>())
                    break;

                current = current.parent;
            }

           
            if (go.GetComponent<MeshFilter>() || go.GetComponent<MeshRenderer>() ||
                go.GetComponentInChildren<MeshFilter>() || go.GetComponentInChildren<MeshRenderer>())
            {
                return current;
            }

            return null;
        }
    }
}
