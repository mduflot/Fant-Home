using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities
{
    public partial class PlayerLookerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // get the nearest player and look in his direction
            // playerPos = ...;
            
            Entities.WithAll<PlayerLooker>().ForEach((ref LocalTransform localTransform) =>
            {
                // localTransform.Rotation = quaternion.LookRotation(playerPos - localTransform.Position, new float3(0, 1, 0));
            }).ScheduleParallel();
        }
    }
}