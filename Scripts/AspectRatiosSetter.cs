#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SmartUserInterface
{
    public static class AspectRatiosSetter
    {
        private const string AspectRatiosPath = "Assets/AspectRatios.asset";

        private static AspectRatios _aspectRatios;

        public static AspectRatios GetAspectRatios()
        {
            if (TryFindAspectRatios() == false)
                CreateCleanAspectRatios();

            return _aspectRatios;
        }

        private static bool TryFindAspectRatios()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(AspectRatios)}");

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _aspectRatios = AssetDatabase.LoadAssetAtPath<AspectRatios>(path);

                if (_aspectRatios != null)
                    return true;
            }

            return false;
        }

        private static void CreateCleanAspectRatios()
        {
            _aspectRatios = ScriptableObject.CreateInstance<AspectRatios>();
            AssetDatabase.CreateAsset(_aspectRatios, AspectRatiosPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"New {nameof(AspectRatios)} asset was created");
        }
    }
}
#endif