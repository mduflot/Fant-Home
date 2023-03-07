using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities
{
    public partial class DestructionSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        private EntityCommandBuffer _entityCommandBuffer;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem =
                World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            _entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();

            EntityQuery projectileEntityQuery = GetEntityQuery(ComponentType.ReadOnly<ProjectileTag>(),
                ComponentType.ReadOnly<LocalTransform>());

            NativeArray<Entity> projectileEntityNativeArray = projectileEntityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<LocalTransform> projectileLocalTransformNativeArray =
                projectileEntityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

            Entities.WithAll<EnemyTag>().ForEach((Entity entity, in LocalTransform localTransform) =>
            {
                for (int i = 0; i < projectileLocalTransformNativeArray.Length; i++)
                {
                    if (math.length(localTransform.Position - projectileLocalTransformNativeArray[i].Position) < 1)
                    {
                        _entityCommandBuffer.DestroyEntity(entity);
                        _entityCommandBuffer.DestroyEntity(projectileEntityNativeArray[i]);
                    }
                }
            }).WithoutBurst().Run();

            projectileEntityNativeArray.Dispose();
            projectileLocalTransformNativeArray.Dispose();
        }
    }
}