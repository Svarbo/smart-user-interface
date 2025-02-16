using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartUserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class SmartRectTransform : MonoBehaviour
    {
        private const string AspectRatiosPath = "Assets/AspectRatios.asset";

        [SerializeField] private AspectRatios _aspectRatios;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private List<SavedRect> _savedRects = new List<SavedRect>();
        [SerializeField] private bool _wasSaved;

        private SavedRect _currentRectConfig;

        private float _currentAspectRatioQuotient => (float)Camera.main.pixelWidth / Camera.main.pixelHeight;
        private bool _isContainsCurrentAspectRatio => _savedRects.Any(savedRect => savedRect.Quotient == _currentAspectRatioQuotient);

        private void Awake() =>
            SetCorrectRectValues();

#if UNITY_EDITOR
        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (TryFindAspectRatios() == false)
                CreateCleanAspectRatios();
        }

        private void OnDrawGizmos()
        {
            SaveCurrentRectConfig();

            if (_wasSaved == false)
            {
                WindowResolutionTracker.OnScreenAspectRatioChanged += SetCorrectRectValues;
                _wasSaved = true;
            }
        }

        private bool TryFindAspectRatios()
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

        private void CreateCleanAspectRatios()
        {
            _aspectRatios = ScriptableObject.CreateInstance<AspectRatios>();
            AssetDatabase.CreateAsset(_aspectRatios, AspectRatiosPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"Created new {nameof(AspectRatios)} asset");
        }

        private void SaveCurrentRectConfig()
        {
            if (_aspectRatios.IsContainsAspectRatio(_currentAspectRatioQuotient))
            {
                if (_isContainsCurrentAspectRatio)
                {
                    _currentRectConfig = _savedRects.Where(savedRect => savedRect.Quotient == _currentAspectRatioQuotient).FirstOrDefault();
                    _currentRectConfig.SaveData(_currentAspectRatioQuotient, _rectTransform);
                }
                else
                {
                    _currentRectConfig = new SavedRect();
                    _currentRectConfig.SaveData(_currentAspectRatioQuotient, _rectTransform);

                    _savedRects.Add(_currentRectConfig);
                }
            }
            else
            {
                Debug.LogError($"Error: An attempt to save the rect transform values to strange aspect ratio {_currentAspectRatioQuotient}!");
            }
        }
#endif

        private void SetCorrectRectValues()
        {
            SavedRect savedRect;

            if (_isContainsCurrentAspectRatio)
                savedRect = _savedRects.Where(savedRect => savedRect.Quotient == _currentAspectRatioQuotient).First();
            else
                savedRect = GetNearestSavedRect(_currentAspectRatioQuotient);

            if (savedRect != null)
                savedRect.UseData(_rectTransform);
        }

        private SavedRect GetNearestSavedRect(float currentAspectRatioQuotient)
        {
            if (_savedRects.Count != 0)
            {
                SavedRect nearestSavedRect = _savedRects[0];
                float minErrorRate = _savedRects[0].Quotient;
                float verifiableErrorRate;

                for (int i = 0; i < _savedRects.Count; i++)
                {
                    verifiableErrorRate = Math.Abs(currentAspectRatioQuotient - _savedRects[i].Quotient);

                    if (verifiableErrorRate < minErrorRate)
                    {
                        minErrorRate = verifiableErrorRate;
                        nearestSavedRect = _savedRects[i];
                    }
                }

                return nearestSavedRect;
            }
            else
            {
                return null;
            }
        }
    }
}