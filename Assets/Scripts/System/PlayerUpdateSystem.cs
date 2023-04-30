using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct PlayerUpdateSystem : ISystem
    {
        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerInputData>();
            state.RequireForUpdate<PlayerData>();
            state.RequireForUpdate<LocalTransform>();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var playerInputData = SystemAPI.GetSingleton<PlayerInputData>();
            var movement = new float3(playerInputData.Move.x, 0f, playerInputData.Move.y) * deltaTime;
            
            foreach (var (playerParam, localTransform) in SystemAPI.Query<RefRO<PlayerData>, RefRW<LocalTransform>>())
            {
                var moveSpeed = playerParam.ValueRO.MoveSpeed;
                localTransform.ValueRW.Position += movement * moveSpeed;
            }
        }
    }
}