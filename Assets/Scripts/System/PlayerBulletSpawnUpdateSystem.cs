using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct PlayerBulletSpawnUpdateSystem : ISystem
    {
        private EntityQuery _entityQuery;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            var components = new NativeArray<ComponentType>(2, Allocator.Temp);
            components[0] = ComponentType.ReadWrite<PlayerBulletSpawnData>();
            components[1] = ComponentType.ReadOnly<LocalToWorld>();

            _entityQuery = state.GetEntityQuery(components);
            components.Dispose();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp, PlaybackPolicy.MultiPlayback);

            var deltaTime = SystemAPI.Time.DeltaTime;
            var entities = _entityQuery.ToEntityArray(Allocator.TempJob);
            foreach (var entity in entities)
            {
                var spawner = entityManager.GetComponentData<PlayerBulletSpawnData>(entity);
                if (spawner.BulletSpawnCurrentTime > 0f)
                {
                    spawner.BulletSpawnCurrentTime -= deltaTime;
                    entityManager.SetComponentData(entity, spawner);
                    continue;
                }

                spawner.BulletSpawnCurrentTime = spawner.BulletSpawnInterval;
                entityManager.SetComponentData(entity, spawner);

                var spawnerLtw = entityManager.GetComponentData<LocalToWorld>(entity);
                var bulletEntity = entityCommandBuffer.Instantiate(spawner.BulletPrototype);
                var bulletTransform = LocalTransform.FromPositionRotation(spawnerLtw.Position, spawnerLtw.Rotation);
                entityCommandBuffer.AddComponent(bulletEntity, bulletTransform);
            }

            entityCommandBuffer.Playback(entityManager);
            entities.Dispose();
            entityCommandBuffer.Dispose();
        }
    }
}