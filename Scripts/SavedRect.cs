using System;
using UnityEngine;

namespace SmartUserInterface
{
    [Serializable]
    public class SavedRect
    {
        [field: SerializeField] public float Quotient { get; private set; }
        [field: SerializeField] public Vector3 AnchoredPosition { get; private set; }
        [field: SerializeField] public Vector2 SizeDelta { get; private set; }
        [field: SerializeField] public Vector2 AnchorMin { get; private set; }
        [field: SerializeField] public Vector2 AnchorMax { get; private set; }
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        [field: SerializeField] public Vector3 Rotation { get; private set; }
        [field: SerializeField] public Vector3 Scale { get; private set; }

        public void SaveData(float quotient, RectTransform rectTransform)
        {
            if (rectTransform == null)
                return;

            Quotient = quotient;
            AnchoredPosition = rectTransform.anchoredPosition;
            SizeDelta = rectTransform.sizeDelta;
            AnchorMin = rectTransform.anchorMin;
            AnchorMax = rectTransform.anchorMax;
            Pivot = rectTransform.pivot;
            Rotation = rectTransform.localEulerAngles;
            Scale = rectTransform.localScale;
        }

        public void UseData(RectTransform rectTransform)
        {
            if (rectTransform == null)
                return;

            rectTransform.anchoredPosition = AnchoredPosition;
            rectTransform.sizeDelta = SizeDelta;
            rectTransform.anchorMin = AnchorMin;
            rectTransform.anchorMax = AnchorMax;
            rectTransform.pivot = Pivot;
            rectTransform.localEulerAngles = Rotation;
            rectTransform.localScale = Scale;
        }
    }
}