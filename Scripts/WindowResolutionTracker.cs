#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace SmartUserInterface
{
    [InitializeOnLoad]
    public class WindowResolutionTracker
    {
        private static Vector2Int _currentResolution;
        private static Vector2Int _lastResolution;

        public static event Action OnScreenAspectRatioChanged;

        private static float _currentResolutionQuotient => (float)_currentResolution.x / _currentResolution.y;
        private static float _lastResolutionQuotient => (float)_lastResolution.x / _lastResolution.y;

        static WindowResolutionTracker()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            _currentResolution.x = Camera.main.pixelWidth;
            _currentResolution.y = Camera.main.pixelHeight;

            if (_lastResolution == null)
            {
                _lastResolution.x = Camera.main.pixelWidth;
                _lastResolution.y = Camera.main.pixelHeight;
            }

            if (_currentResolutionQuotient != _lastResolutionQuotient)
            {
                _lastResolution = _currentResolution;
                Debug.Log("OnScreenAspectRatioChanged");
                OnScreenAspectRatioChanged?.Invoke();
            }
        }
    }
}
#endif