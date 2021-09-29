using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnchorAutoFit 
{
    [MenuItem("GameObject/UGUI/SetNativeSize", false, 10)]
    static void SetImgNative()
    {
        Image img = Selection.activeGameObject.GetComponent<Image>();
        if (img == null) return;
        img.SetNativeSize();
    }

    [MenuItem("GameObject/UGUI/Anchors to Corners", false, 10)]
    static void AnchorsToCorners()
    {
        RectTransform rect = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;


        if (rect == null || pt == null) return;

        Vector2 newAnchorsMin = new Vector2(rect.anchorMin.x + rect.offsetMin.x / pt.rect.width,
                                            rect.anchorMin.y + rect.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(rect.anchorMax.x + rect.offsetMax.x / pt.rect.width,
                                            rect.anchorMax.y + rect.offsetMax.y / pt.rect.height);

        rect.anchorMin = newAnchorsMin;
        rect.anchorMax = newAnchorsMax;
        rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
    }

    
    [MenuItem("GameObject/UGUI/Corners to Anchors", false, 10)]
    static void CornersToAnchors()
    {
        RectTransform rect = Selection.activeTransform as RectTransform;

        if (rect == null) return;

        rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
    }
    //[InitializeOnLoadMethod]
    //static void StartInitializeOnLoadMethod()
    //{
    //    EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    //}

    //static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    //{
    //    if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
    //        && Event.current.button == 1 && Event.current.type <= EventType.MouseUp)
    //    {
    //        GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    //        //这里可以判断selectedGameObject的条件
    //        if (selectedGameObject)
    //        {
    //            Vector2 mousePosition = Event.current.mousePosition;

    //            EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "UGUI", null);
    //            Event.current.Use();
    //        }
    //    }
    //}
}
