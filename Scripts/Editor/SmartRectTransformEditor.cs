#if UNITY_EDITOR
using UnityEditor;

namespace SmartUserInterface
{
    [CustomEditor(typeof(SmartRectTransform))]
    public class SmartRectTransformEditor : Editor
    {
        private SmartRectTransform _smartRectTransform;

        private void OnEnable()
        {
            _smartRectTransform = target as SmartRectTransform;
        }

        private void OnDisable()
        {
            _smartRectTransform.SaveCurrentRectConfig();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
#endif