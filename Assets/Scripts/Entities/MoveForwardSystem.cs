using Unity.Entities;
using Unity.Transforms;

namespace Entities
{
    public partial class MoveForwardSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float time = World.Time.DeltaTime;
        
            Entities.WithNone<EnemyTag>().ForEach((ref LocalTransform localTransform, in MoveForward moveForward) =>
            {
                localTransform.Position += localTransform.Forward() * moveForward.Speed * time;
            }).ScheduleParallel();
        
            Entities.WithAll<EnemyTag>().ForEach((ref LocalTransform localTransform, in Avoidance avoidance, in MoveForward moveForward) =>
            {
                localTransform.Position += (localTransform.Forward() * moveForward.Speed + avoidance.AvoidanceDirection * avoidance.AvoidanceStrength) * time;
            }).ScheduleParallel();
        }
    }
}