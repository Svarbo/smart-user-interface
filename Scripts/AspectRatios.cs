using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartUserInterface
{
    [CreateAssetMenu(fileName = "AspectRatios", menuName = "Tools/AspectRatios")]
    public class AspectRatios : ScriptableObject
    {
        [SerializeField] private List<AspectRatio> _aspectRatios = new List<AspectRatio>();

        public bool IsContainsAspectRatio(float currentAspectRatioQuotient) =>
            _aspectRatios.Any(aspectRatio => aspectRatio.Quotient == currentAspectRatioQuotient);

        private AspectRatio GetNearestAspectRatio(AspectRatio aspectRatio)
        {
            AspectRatio nearestAspectRatio = null;
            float minErrorRate = float.MaxValue;
            float verifiableErrorRate;

            for (int i = 0; i < _aspectRatios.Count; i++)
            {
                verifiableErrorRate = Mathf.Abs(aspectRatio.Quotient - _aspectRatios[i].Quotient);

                if (verifiableErrorRate < minErrorRate)
                {
                    minErrorRate = verifiableErrorRate;
                    nearestAspectRatio = _aspectRatios[i];
                }
            }

            return nearestAspectRatio;
        }
    }
}