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
    }
}