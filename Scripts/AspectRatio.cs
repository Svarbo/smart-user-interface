using System;
using UnityEngine;

namespace SmartUserInterface
{
    [Serializable]
    public class AspectRatio
    {
        [field: SerializeField] public float Width { get; private set; }
        [field: SerializeField] public float Heigth { get; private set; }
        [field: SerializeField] public DeviceType DeviceTypes { get; private set; }

        public float Quotient => Width / Heigth;
    }
}