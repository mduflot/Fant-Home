using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities
{
    public partial class AvoidanceSystem : SystemBase
    {
        private const int _quadrantCellSize = 10;
        private const int _quadrantYMultiplier = 1000;

        private static NativeMultiHashMap<int, float3> s_cellVsEntityPosition;

        protected override void OnCreate()
        {
            s_cellVsEntityPosition = new NativeMultiHashMap<int, float3>(0, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            EntityQuery entityQuery = GetEntityQuery(typeof(EnemyTag));
            int entityCount = entityQuery.CalculateEntityCount();
            s_cellVsEntityPosition.Dispose(); // Dispose of the old map
            s_cellVsEntityPosition = new NativeMultiHashMap<int, float3>(entityCount, Allocator.Persistent);

            NativeMultiHashMap<int, float3>.ParallelWriter cellVsEntityPositionParallel =
                s_cellVsEntityPosition.AsParallelWriter();

            Entities.ForEach((ref Avoidance avoidance, in LocalTransform localTransform) =>
            {
                cellVsEntityPositionParallel.Add(GetUnitqueKeyForThisPosition(localTransform.Position),
                    localTransform.Position);
            }).ScheduleParallel();

            NativeMultiHashMap<int, float3> cellVsEntityPositionForJob = s_cellVsEntityPosition;

            Entities.WithReadOnly(cellVsEntityPositionForJob).ForEach(
                (ref Avoidance avoidance, in LocalTransform localTransform) =>
                {
                    int key = GetUnitqueKeyForThisPosition(localTransform.Position);
                    NativeMultiHashMapIterator<int> nmhKeyIterator;
                    float3 currentLocationToCheck;

                    int total = 0;
                    avoidance.AvoidanceDirection = float3.zero;

                    if (cellVsEntityPositionForJob.TryGetFirstValue(key, out currentLocationToCheck,
                            out nmhKeyIterator))
                    {
                        do
                        {
                            if (!localTransform.Position.Equals(currentLocationToCheck))
                            {
                                float currentDistance =
                                    math.sqrt(math.lengthsq(localTransform.Position - currentLocationToCheck));

                                if (currentDistance < avoidance.AvoidanceDistance)
                                {
                                    float3 distanceFromTo = localTransform.Position - currentLocationToCheck;

                                    avoidance.AvoidanceDirection += math.normalize(distanceFromTo / currentDistance);

                                    total++;
                                }
                            }
                        } while (cellVsEntityPositionForJob.TryGetNextValue(out currentLocationToCheck,
                                     ref nmhKeyIterator));
                    }
                }).ScheduleParallel();
        }

        [BurstCompile]
        private static int GetUnitqueKeyForThisPosition(float3 position)
        {
            return (int)(math.floor(position.x / _quadrantCellSize) +
                         (_quadrantYMultiplier * math.floor(position.z / _quadrantCellSize)));
        }
    }
}