using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct HitEffectPlayingSystem : ISystem
    {
        private EntityQuery _entityQuery;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            var components = new NativeArray<ComponentType>(1, Allocator.Temp);
            components[0] = ComponentType.ReadWrite<HitEffectPlayingData>();

            _entityQuery = state.GetEntityQuery(components);
            components.Dispose();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.MultiPlayback);

            var deltaTime = SystemAPI.Time.DeltaTime;
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                var hitEffectPlayingData = entityManager.GetComponentData<HitEffectPlayingData>(entity);
                if (hitEffectPlayingData.AutoDestroyTime <= 0f)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }

                hitEffectPlayingData.AutoDestroyTime -= deltaTime;
                entityManager.SetComponentData(entity, hitEffectPlayingData);
            }

            entityCommandBuffer.Playback(entityManager);
            entities.Dispose();
            entityCommandBuffer.Dispose();
        }
    }
}