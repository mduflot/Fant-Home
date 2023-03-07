using Unity.Entities;
using Unity.Mathematics;

namespace Entities
{
    public struct Avoidance : IComponentData
    {
        public float3 AvoidanceDirection;
        public float AvoidanceStrength;
        public float AvoidanceDistance;
    }
}