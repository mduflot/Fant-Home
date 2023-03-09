using Unity.Entities;

namespace Entities
{
    public struct PlayerLooker : IComponentData
    {
        public int CurrentCheckpoint;
        public bool IsTargeting;
        public float DistanceAlert;
    }
}