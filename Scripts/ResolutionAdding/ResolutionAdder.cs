using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SmartUserInterface
{
    public static class ResolutionAdder
    {
        private static object _gameViewSizesInstance;
        private static MethodInfo _getGroup;

        static ResolutionAdder()
        {
            Type sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            Type singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            PropertyInfo instanceProp = singleType.GetProperty("instance");

            _getGroup = sizesType.GetMethod("GetGroup");
            _gameViewSizesInstance = instanceProp.GetValue(null, null);
        }

        public enum GameViewSizeType
        {
            AspectRatio, FixedResolution
        }

        [MenuItem("Test/AddSize")]
        public static void AddTestSize()
        {
            AddCustomSize(GameViewSizeType.AspectRatio, GameViewSizeGroupType.Standalone, 123, 456, "Test size");
        }

        [MenuItem("Test/SizeTextQuery")]
        public static void SizeTextQueryTest()
        {
            Debug.Log(IsSizeExists(GameViewSizeGroupType.Standalone, "Test size"));
        }

        [MenuItem("Test/Query16:9Test")]
        public static void WidescreenQueryTest()
        {
            Debug.Log(IsSizeExists(GameViewSizeGroupType.Standalone, "16:9"));
        }

        [MenuItem("Test/Set16:9")]
        public static void SetWidescreenTest()
        {
            SetSize(FindSize(GameViewSizeGroupType.Standalone, "16:9"));
        }

        [MenuItem("Test/SetTestSize")]
        public static void SetTestSize()
        {
            int index = FindSize(GameViewSizeGroupType.Standalone, 123, 456);
            if (index != -1)
                SetSize(index);
        }

        public static void SetSize(int index)
        {
            Type gameViewWindowType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            PropertyInfo selectedSizeIndexProp = gameViewWindowType.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            EditorWindow gameViewWindow = EditorWindow.GetWindow(gameViewWindowType);

            selectedSizeIndexProp.SetValue(gameViewWindow, index, null);
        }

        [MenuItem("Test/SizeDimensionsQuery")]
        public static void SizeDimensionsQueryTest()
        {
            Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, 123, 456));
        }

        public static void AddAndSelectCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            AddCustomSize(viewSizeType, sizeGroupType, width, height, text);
            int idx = FindSize(GameViewSizeGroupType.Standalone, width, height);
            SetSize(idx);
        }

        public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            object group = GetGroup(sizeGroupType);
            MethodInfo addCustomSize = _getGroup.ReturnType.GetMethod("AddCustomSize");
            string assemblyName = "UnityEditor.dll";
            Assembly assembly = Assembly.Load(assemblyName);
            Type gameViewSize = assembly.GetType("UnityEditor.GameViewSize");
            Type gameViewSizeType = assembly.GetType("UnityEditor.GameViewSizeType");

            ConstructorInfo constructorInfo = gameViewSize.GetConstructor(new Type[]
                {
                 gameViewSizeType,
                 typeof(int),
                 typeof(int),
                 typeof(string)
                });

            object newSize = constructorInfo.Invoke(new object[] { (int)viewSizeType, width, height, text });
            addCustomSize.Invoke(group, new object[] { newSize });
        }

        public static bool IsSizeExists(GameViewSizeGroupType sizeGroupType, string text)
        {
            return FindSize(sizeGroupType, text) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            object group = GetGroup(sizeGroupType);
            MethodInfo getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            string[] displayTexts = getDisplayTexts.Invoke(group, null) as string[];

            for (int i = 0; i < displayTexts.Length; i++)
            {
                string display = displayTexts[i];
                int pren = display.IndexOf('(');

                if (pren != -1)
                    display = display.Substring(0, pren - 1);

                if (display == text)
                    return i;
            }

            return -1;
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            return FindSize(sizeGroupType, width, height) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            object group = GetGroup(sizeGroupType);
            Type groupType = group.GetType();
            MethodInfo getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            MethodInfo getCustomCount = groupType.GetMethod("GetCustomCount");
            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            MethodInfo getGameViewSize = groupType.GetMethod("GetGameViewSize");
            Type gvsType = getGameViewSize.ReturnType;
            PropertyInfo widthProp = gvsType.GetProperty("width");
            PropertyInfo heightProp = gvsType.GetProperty("height");
            object[] indexValue = new object[1];

            for (int i = 0; i < sizesCount; i++)
            {
                indexValue[0] = i;

                object size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);

                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }

            return -1;
        }

        [MenuItem("Test/LogCurrentGroupType")]
        public static void LogCurrentGroupType()
        {
            Debug.Log(GetCurrentGroupType());
        }

        public static GameViewSizeGroupType GetCurrentGroupType()
        {
            PropertyInfo getCurrentGroupTypeProp = _gameViewSizesInstance.GetType().GetProperty("currentGroupType");

            return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(_gameViewSizesInstance, null);
        }

        private static object GetGroup(GameViewSizeGroupType type)
        {
            return _getGroup.Invoke(_gameViewSizesInstance, new object[] { (int)type });
        }
    }
}