using UnityEditor;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Window/Custom Editor Window")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorWindow>("Custom Window");
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Debug.Log($"Selected object with RectTransform: {selectedObject.name}");
            }
        }
        else
        {
            Debug.Log("No object selected.");
        }
    }
}