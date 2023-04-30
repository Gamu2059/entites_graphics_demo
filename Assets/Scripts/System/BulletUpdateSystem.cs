using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct BulletUpdateSystem : ISystem
    {
        private EntityQuery _entityQuery;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            var components = new NativeArray<ComponentType>(2, Allocator.Temp);
            components[0] = ComponentType.ReadWrite<BulletData>();
            components[1] = ComponentType.ReadWrite<LocalTransform>();
            
            _entityQuery = state.GetEntityQuery(components);
            components.Dispose();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var ecb = new EntityCommandBuffer(Allocator.Temp, PlaybackPolicy.MultiPlayback);

            var deltaTime = SystemAPI.Time.DeltaTime;
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                var bulletData = entityManager.GetComponentData<BulletData>(entity);
                var localTransform = entityManager.GetComponentData<LocalTransform>(entity);

                var speed = bulletData.Speed;
                var movement = speed * deltaTime;
                bulletData.AutoDestroyDistance -= movement;
                bulletData.PrePosition = localTransform.Position;

                var deltaPosition = localTransform.Forward() * movement;
                localTransform.Position += deltaPosition;
                bulletData.CurrentPosition = localTransform.Position;

                entityManager.SetComponentData(entity, bulletData);
                entityManager.SetComponentData(entity, localTransform);

                if (bulletData.AutoDestroyDistance <= 0f)
                {
                    entityManager.DestroyEntity(entity);
                }
            }

            ecb.Playback(entityManager);
            entities.Dispose();
            ecb.Dispose();
        }
    }
}