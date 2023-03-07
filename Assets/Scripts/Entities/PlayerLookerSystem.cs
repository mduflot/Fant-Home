using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Entities
{
    public partial class PlayerLookerSystem : SystemBase
    {
        private static NativeList<float3> s_gameObjectPlayers;

        protected override void OnCreate()
        {
            s_gameObjectPlayers = new NativeList<float3>(0, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            s_gameObjectPlayers.Dispose();
            s_gameObjectPlayers = new NativeList<float3>(players.Length, Allocator.Persistent);

            NativeList<float3>.ParallelWriter gameObjectPlayersParallel = s_gameObjectPlayers.AsParallelWriter();

            foreach (var player in players)
            {
                gameObjectPlayersParallel.AddNoResize(player.transform.position);
            }

            NativeList<float3> gameObjectPlayersJob = s_gameObjectPlayers;

            Entities.WithReadOnly(gameObjectPlayersJob).WithAll<PlayerLooker>().ForEach(
                (ref LocalTransform localTransform) =>
                {
                    if (gameObjectPlayersJob.Length < 1) return;
                    float currentDistance = math.sqrt(math.lengthsq(localTransform.Position - gameObjectPlayersJob[0]));
                    float3 playerPos = gameObjectPlayersJob[0];
                        
                    foreach (var player in gameObjectPlayersJob)
                    {
                        if (currentDistance >
                            math.sqrt(math.lengthsq(localTransform.Position - player)))
                        {
                            playerPos = player;
                        }
                    }
                    
                    localTransform.Rotation = quaternion.LookRotation(playerPos - localTransform.Position,
                        new float3(0, 1, 0));
                }).ScheduleParallel();
        }
    }
}