using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities
{
    public partial class PatrolSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            EntityQuery waypointsEntityQuery = GetEntityQuery(ComponentType.ReadOnly<WaypointTag>(),
                ComponentType.ReadOnly<LocalTransform>());

            NativeArray<Entity> waypointsEntityNativeArray = waypointsEntityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<LocalTransform> waypointsLocalTransformNativeArray =
                waypointsEntityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

            Entities.WithAll<EnemyTag>().ForEach((Entity entity, ref LocalTransform localTransform, in PlayerLooker playerLooker) =>
            {
                if (math.length(localTransform.Position -
                                waypointsLocalTransformNativeArray[playerLooker.CurrentCheckpoint].Position) < 1)
                {
                    int currentCheckpoint = playerLooker.CurrentCheckpoint;
                    if (currentCheckpoint + 1 > waypointsLocalTransformNativeArray.Length - 1) currentCheckpoint = 0;
                    else currentCheckpoint++;
                    EntityManager.SetComponentData(entity, new PlayerLooker()
                    {
                        CurrentCheckpoint = currentCheckpoint,
                        DistanceAlert = playerLooker.DistanceAlert,
                        IsTargeting = playerLooker.IsTargeting
                    });
                }
            }).WithoutBurst().Run();
        }
    }
}