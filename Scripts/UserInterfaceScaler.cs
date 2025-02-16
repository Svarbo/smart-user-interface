using UnityEngine;

namespace SmartUserInterface
{
    public class UserInterfaceScaler : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnValidate() =>
            AddSavedRectToChilds();
#endif

        private void AddSavedRectToChilds()
        {
            RectTransform[] rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
            GameObject rectTransformObject;
            bool isContainsSmartRectTransform;

            for (int i = 0; i < rectTransforms.Length; i++)
            {
                rectTransformObject = rectTransforms[i].gameObject;
                isContainsSmartRectTransform = rectTransformObject.TryGetComponent(out SmartRectTransform smartRectTransform);

                if (isContainsSmartRectTransform == false)
                    rectTransformObject.AddComponent<SmartRectTransform>();
            }
        }
    }
}