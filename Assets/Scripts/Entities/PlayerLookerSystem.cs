using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial class PlayerLookerSystem : MonoBehaviour
{
    private static NativeList<float3> s_gameObjectPlayers;
    private GameObject[] _players;

    protected void Update()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        s_gameObjectPlayers = new NativeList<float3>(0, Allocator.Persistent);
        s_gameObjectPlayers.Dispose();
        s_gameObjectPlayers = new NativeList<float3>(_players.Length, Allocator.Persistent);

        NativeList<float3>.ParallelWriter gameObjectPlayersParallel = s_gameObjectPlayers.AsParallelWriter();

        foreach (var player in _players)
        {
            gameObjectPlayersParallel.AddNoResize(player.transform.position);
        }

        NativeList<float3> gameObjectPlayersJob = s_gameObjectPlayers;

        if (gameObjectPlayersJob.Length < 1) return;
        float currentDistance = math.sqrt(math.lengthsq(transform.localPosition - (Vector3)gameObjectPlayersJob[0]));
        float3 playerPos = gameObjectPlayersJob[0];
            
        foreach (var player in gameObjectPlayersJob)
        {
            if (currentDistance >
                math.sqrt(math.lengthsq(transform.localPosition - (Vector3)player)))
            {
                playerPos = player;
            }

            NativeList<float3> gameObjectPlayersJob = s_gameObjectPlayers;

            EntityQuery waypointsEntityQuery = GetEntityQuery(ComponentType.ReadOnly<WaypointTag>(),
                ComponentType.ReadOnly<LocalTransform>());

            NativeArray<Entity> waypointsEntityNativeArray = waypointsEntityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<LocalTransform> waypointsLocalTransformNativeArray =
                waypointsEntityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

            Entities.WithReadOnly(gameObjectPlayersJob).WithAll<PlayerLooker>().ForEach(
                (Entity entity, ref LocalTransform localTransform, in PlayerLooker playerLooker) =>
                {
                    if (gameObjectPlayersJob.Length < 1) return;
                    float currentDistance = math.sqrt(math.lengthsq(localTransform.Position - gameObjectPlayersJob[0]));
                    float3 playerPos;

                    foreach (var player in gameObjectPlayersJob)
                    {
                        if (currentDistance < playerLooker.DistanceAlert)
                        {
                            playerPos = player;
                            EntityManager.SetComponentData(entity, new PlayerLooker()
                            {
                                DistanceAlert = playerLooker.DistanceAlert,
                                IsTargeting = true,
                                CurrentCheckpoint = playerLooker.CurrentCheckpoint
                            });
                            localTransform.Rotation = quaternion.LookRotation(playerPos - localTransform.Position,
                                new float3(0, 1, 0));
                        }
                        else
                        {
                            EntityManager.SetComponentData(entity, new PlayerLooker()
                            {
                                DistanceAlert = playerLooker.DistanceAlert,
                                IsTargeting = false,
                                CurrentCheckpoint = playerLooker.CurrentCheckpoint
                            });
                            localTransform.Rotation = quaternion.LookRotation(
                                waypointsLocalTransformNativeArray[playerLooker.CurrentCheckpoint].Position -
                                localTransform.Position,
                                new float3(0, 1, 0));
                        }
                    }
                }).WithoutBurst().Run();
        }
        
        transform.rotation = quaternion.LookRotation((Vector3)playerPos - transform.localPosition,
            new float3(0, 1, 0));
    }
}
