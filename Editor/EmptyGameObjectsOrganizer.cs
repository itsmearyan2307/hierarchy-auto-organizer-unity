using Codice.Client.Common.GameUI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace itsnotmearyan.HierarchyOraganizerTool
{
    public static class EmptyGameObjectsOrganizer
    {
        private static TransformRootMarker transformRootMarker;
        private static ManagersRootMarker managersRootMarker;
        private static readonly List<GameObject> referenced = new();

        public static void PrepareEmptyGameObjects()
        {
            if (referenced.Count > 0)
                referenced.Clear();

            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

            CollectReferencedGameObjects();
            DeleteGameObjects(allObjects);
        }

        private static void CollectReferencedGameObjects()
        {
            foreach (var mono in Object.FindObjectsOfType<MonoBehaviour>())
            {
                if (mono == null) continue;

                SerializedObject so = new SerializedObject(mono);
                SerializedProperty prop = so.GetIterator();

                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        GameObject goRef = null;

                        if (prop.objectReferenceValue is GameObject directRef)
                            goRef = directRef;
                        else if (prop.objectReferenceValue is Component compRef)
                            goRef = compRef.gameObject;

                        if (goRef != null &&
                            goRef.transform.childCount == 0 &&
                            goRef.GetComponents<Component>().Length < 3 &&
                            !goRef.GetComponent<Light>()
                            && !goRef.GetComponent<AudioSource>())
                        {
                            if (!referenced.Contains(goRef))
                                referenced.Add(goRef);
                        }
                    }
                }
            }
        }

        public static void DeleteGameObjects(GameObject[] allObjects)
        {
            foreach (GameObject go in allObjects)
            {
                if (go.transform.childCount == 0 &&
                    go.GetComponents<Component>().Length == 1 &&
                    go.scene.IsValid() &&
                    !referenced.Contains(go) &&
                    go.transform.parent == null)
                {
                    Debug.Log($"Deleted:{go.name}");
                    Undo.DestroyObjectImmediate(go);

                }
                else if ((go.GetComponent<EnvironmentRootMarker>() ||
                          go.GetComponent<UIRootMarker>() ||
                          go.GetComponent<TransformRootMarker>() ||
                          go.GetComponent<ManagersRootMarker>()) &&
                          go.transform.childCount == 0)
                {
                    Debug.Log($"Deleted:{go.name}");
                    Undo.DestroyObjectImmediate(go);

                }
            }
        }

        public static void ArrangeEmptyGameObjects()
        {
            CollectReferencedGameObjects();
            if (referenced.Count == 0)
                return;

            GameObject managersContainer = null;
            GameObject transformContainer = null;

            foreach (GameObject go in referenced)
            {
                if (go == null || go.transform.parent != null)
                    continue;


                if (transformContainer != null && go.transform.IsChildOf(transformRootMarker.transform))
                    continue;


                if (managersContainer != null && go.transform.IsChildOf(managersContainer.transform))
                    continue;




                if (HasCustomMonoBehaviour(go))
                {
                    if (managersContainer == null)
                    {
                        ManagersRootMarker existingMarker = Object.FindObjectOfType<ManagersRootMarker>();
                        if (existingMarker != null)
                        {
                            managersContainer = existingMarker.gameObject;
                        }
                        else
                        {
                            managersContainer = new GameObject("--------MANAGERS--------");
                            managersContainer.AddComponent<ManagersRootMarker>();
                            Debug.Log($"Created MANAGERS root");
                        }

                        managersRootMarker = managersContainer.GetComponent<ManagersRootMarker>();
                    }

                    Undo.SetTransformParent(go.transform, managersRootMarker.transform, "Changed Parent");
                    Debug.Log($"Grouped in MANAGERS: {go.name}");
                }
                else
                {

                    if (transformContainer == null)
                    {
                        TransformRootMarker existingTransformMarker = Object.FindObjectOfType<TransformRootMarker>();
                        if (existingTransformMarker != null)
                        {
                            transformContainer = existingTransformMarker.gameObject;
                        }
                        else
                        {
                            transformContainer = new GameObject("--------EMPTY TRANSFORMS/TARGETS----------");
                            transformContainer.AddComponent<TransformRootMarker>();
                            Debug.Log($"Created EMPTY/TRANSFORMS root");
                        }

                        transformRootMarker = transformContainer.GetComponent<TransformRootMarker>();
                    }



                    Debug.Log($"Grouped in EMPTY TRANSFORMS: {go.name}");
                    Undo.SetTransformParent(go.transform, transformRootMarker.transform, "Changed Parent");


                }
            }
            CheckForAdditionalManagers();
        }

        private static bool HasCustomMonoBehaviour(GameObject go)
        {
            foreach (var mb in go.GetComponents<MonoBehaviour>())
            {
                if (mb == null) continue;

                var type = mb.GetType();
                if (type.Namespace == null || !type.Namespace.StartsWith("UnityEngine"))
                    return true;
            }
            return false;
        }
        private static bool IsLikelyManager(MonoBehaviour mb)
        {
            if (mb == null) return false;

            var type = mb.GetType();
            return type.Name.Contains("Manager") || type.Name.Contains("Controller") ||
                   (type.Namespace != null && type.Namespace.Contains("Managers"));
        }






        private static void CheckForAdditionalManagers()
        {

            GameObject[] allobjects = Object.FindObjectsOfType<GameObject>();
            if (managersRootMarker == null)
            {
                managersRootMarker = new GameObject("--------MANAGERS--------").AddComponent<ManagersRootMarker>(); ;
                Debug.Log($"Created MANAGERS root");
               
            }
            foreach (GameObject obj in allobjects)
            {
                if (obj.transform.childCount == 0 && obj.GetComponents<Component>().Length < 3 && obj.transform.parent == null)
                {
                    var scripts = obj.GetComponents<MonoBehaviour>();
                    foreach (var script in scripts)
                    {
                        if (IsLikelyManager(script))
                        {
                            Undo.SetTransformParent(obj.transform, managersRootMarker.transform, "Changing Parent ");
                        }


                    }

                }
            }

        }
    }
}