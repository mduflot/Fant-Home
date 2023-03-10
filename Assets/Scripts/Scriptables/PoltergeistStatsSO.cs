using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "new PoltergeistStatsSO", menuName = "Scriptables/GhostsSO/PoltergeistStatsSO")]
    public class PoltergeistStatsSO : GhostStatsSO
    {
        public float DetectionRange;
        public float UnitMakingCD;
        public int NbrUnitsToSpawn;
    }
}