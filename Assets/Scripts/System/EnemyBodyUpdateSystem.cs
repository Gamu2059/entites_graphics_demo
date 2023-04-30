using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(BulletTriggerEventSystem))]
    public partial struct EnemyBodyUpdateSystem : ISystem
    {
        private float4 _hitColor;

        private EntityQuery _entityQuery;
        private ComponentLookup<EnemyBody> _enemyBodyLookup;
        private ComponentLookup<MaterialBaseColor> _materialBaseColorLookup;
        private ComponentLookup<Parent> _parentLookup;
        private ComponentLookup<EnemyData> _enemyDataLookup;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            _hitColor = (Vector4)Color.red;

            var components = new NativeArray<ComponentType>(1, Allocator.Temp);
            components[0] = ComponentType.ReadWrite<EnemyBody>();

            _entityQuery = state.GetEntityQuery(components);
            components.Dispose();

            _enemyBodyLookup = state.GetComponentLookup<EnemyBody>();
            _materialBaseColorLookup = state.GetComponentLookup<MaterialBaseColor>();
            _parentLookup = state.GetComponentLookup<Parent>();
            _enemyDataLookup = state.GetComponentLookup<EnemyData>();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            _enemyBodyLookup.Update(ref state);
            _materialBaseColorLookup.Update(ref state);
            _parentLookup.Update(ref state);
            _enemyDataLookup.Update(ref state);

            var entityManager = state.EntityManager;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp, PlaybackPolicy.MultiPlayback);
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                var enemyBody = _enemyBodyLookup[entity];

                if (!enemyBody.WasSearchEnemyDataEntity)
                {
                    enemyBody.WasSearchEnemyDataEntity = true;
                    enemyBody.EnemyDataEntity = SearchEnemyDataEntity(entity);
                }

                // EnemyDataを持つEntityがなければスキップ
                if (enemyBody.EnemyDataEntity == Entity.Null) continue;
                if (!_enemyDataLookup.TryGetComponent(enemyBody.EnemyDataEntity, out var enemyData)) continue;

                var color = enemyData.IsValidHit ? _hitColor : enemyBody.originalBodyBaseColor;
                if (_materialBaseColorLookup.TryGetComponent(entity, out var materialBaseColor))
                {
                    materialBaseColor.value = color;
                    entityCommandBuffer.SetComponent(entity, materialBaseColor);
                }
                else
                {
                    materialBaseColor.value = color;
                    entityCommandBuffer.AddComponent(entity, materialBaseColor);
                }
            }

            entities.Dispose();
            entityCommandBuffer.Playback(entityManager);
            entityCommandBuffer.Dispose();
        }

        [BurstCompile]
        private Entity SearchEnemyDataEntity(Entity enemyBodyEntity)
        {
            var entity = enemyBodyEntity;

            while (!_enemyDataLookup.HasComponent(entity))
            {
                // ParentもEnemyDataも持っていないところまで階層を遡ったら
                // EnemyBodyより上にEnemyDataを持つEntityは無い
                if (!_parentLookup.TryGetComponent(entity, out var parent))
                {
                    return Entity.Null;
                }

                entity = parent.Value;
            }

            return entity;
        }
    }
}