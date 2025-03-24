using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartUserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class SmartRectTransform : MonoBehaviour
    {
        [SerializeField] private AspectRatios _aspectRatios;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private List<SavedRect> _savedRects = new List<SavedRect>();

        private SavedRect _rectConfig;

        private float _currentAspectRatioQuotient => (float)Camera.main.pixelWidth / Camera.main.pixelHeight;
        private bool _isContainsCurrentAspectRatio => _savedRects.Any(savedRect => savedRect.Quotient == _currentAspectRatioQuotient);

        private void Awake() =>
            SetCorrectRectValues();

#if UNITY_EDITOR
        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
            _aspectRatios = AspectRatiosSetter.GetAspectRatios();

            WindowResolutionTracker.ScreenAspectRatioChanged += OnScreenAspectRatioChanged;
        }

        public void SaveCurrentRectConfig()
        {
            if (_aspectRatios.IsContainsAspectRatio(_currentAspectRatioQuotient))
            {
                if (_isContainsCurrentAspectRatio)
                {
                    _rectConfig = _savedRects.Where(savedRect => savedRect.Quotient == _currentAspectRatioQuotient).FirstOrDefault();
                    _rectConfig.SaveData(_currentAspectRatioQuotient, _rectTransform);
                }
                else
                {
                    _rectConfig = new SavedRect();
                    _rectConfig.SaveData(_currentAspectRatioQuotient, _rectTransform);

                    _savedRects.Add(_rectConfig);
                }
            }
            else
            {
                Debug.LogError($"Error: An attempt to save the rect transform values to strange aspect ratio {_currentAspectRatioQuotient}!");
                SetCorrectRectValues();
            }
        }

        private void OnScreenAspectRatioChanged()
        {
            SavePreviousRectConfiq(WindowResolutionTracker.PreviousResolutionQuotient);
            SetCorrectRectValues();
        }

        private void SavePreviousRectConfiq(float previousResolutionQuotient)
        {
            if (_aspectRatios.IsContainsAspectRatio(previousResolutionQuotient))
            {
                bool isContainsPreviousAspectRatio = _savedRects.Any(savedRect => savedRect.Quotient == previousResolutionQuotient);

                if (isContainsPreviousAspectRatio)
                {
                    _rectConfig = _savedRects.Where(savedRect => savedRect.Quotient == previousResolutionQuotient).FirstOrDefault();
                    _rectConfig.SaveData(previousResolutionQuotient, _rectTransform);
                }
                else
                {
                    _rectConfig = new SavedRect();
                    _rectConfig.SaveData(previousResolutionQuotient, _rectTransform);

                    _savedRects.Add(_rectConfig);
                }
            }
            else
            {
                Debug.LogError($"Error: An attempt to save the rect transform values to strange aspect ratio {previousResolutionQuotient}!");
            }
        }


#endif

        public void SetCorrectRectValues()
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