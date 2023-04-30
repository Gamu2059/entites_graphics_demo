using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct EnemyUpdateSystem : ISystem
    {
        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyData>();
            state.RequireForUpdate<LocalTransform>();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var query in SystemAPI.Query<RefRW<EnemyData>, RefRW<LocalTransform>>())
            {
                var enemyData = query.Item1.ValueRO;

                // 移動制御
                var position = enemyData.OriginPosition;
                var t = 2 * math.PI * enemyData.Frequency * enemyData.CurrentTime + enemyData.OffsetPhase;
                position.z += enemyData.Amplitude * math.sin(t);
                query.Item2.ValueRW.Position = position;

                enemyData.CurrentTime += deltaTime;
                enemyData.HitValidRemainingTime = math.max(enemyData.HitValidRemainingTime - deltaTime, 0f);

                query.Item1.ValueRW = enemyData;
            }
        }
    }
}