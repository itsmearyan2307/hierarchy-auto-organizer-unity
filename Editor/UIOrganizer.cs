using UnityEngine;
namespace itsnotmearyan.HierarchyOraganizerTool
{
    public static class UIOrganizer
    {
        private static UIRootMarker uirootMarker;

        public static void AutoArrangeUIElements()
        {
            Canvas[] allCanvases = Object.FindObjectsOfType<Canvas>();

            if (uirootMarker == null)
                uirootMarker = Object.FindObjectOfType<UIRootMarker>();

            GameObject uiRootObj;

            if (uirootMarker != null)
            {
                uiRootObj = uirootMarker.gameObject;
            }
            else
            {
                uiRootObj = new GameObject("---------UI---------");
                uirootMarker = uiRootObj.AddComponent<UIRootMarker>();
                Debug.Log("Created UI root.");
            }

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas == null || canvas.transform.IsChildOf(uiRootObj.transform))
                    continue;

                canvas.transform.SetParent(uiRootObj.transform, false);
                Debug.Log($"{canvas.name} added to UI root.");
            }
        }
    }
}
