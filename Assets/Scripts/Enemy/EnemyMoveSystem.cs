using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    public partial struct EnemyMoveSystem : ISystem
    {
        void ISystem.OnCreate(ref SystemState state)
        {
        }
        
        void ISystem.OnDestroy(ref SystemState state)
        {
        }
        
        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var query in SystemAPI.Query<RefRW<EnemyParam>, RefRW<LocalTransform>>())
            {
                var enemyParam = query.Item1.ValueRO;
                var time = enemyParam.CurrentTime;
                var p = enemyParam.OriginPosition;
                p.z += enemyParam.Amplitude * math.cos(enemyParam.Frequency * time);
                query.Item2.ValueRW.Position = p;
                
                query.Item1.ValueRW.CurrentTime += deltaTime;
            }
        }
    }
}