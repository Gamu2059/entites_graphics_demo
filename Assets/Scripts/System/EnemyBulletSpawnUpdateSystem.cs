using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationCharaSystemGroup))]
    public partial struct EnemyBulletSpawnUpdateSystem : ISystem
    {
        private EntityQuery _entityQuery;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyBulletSpawnData>();
            state.RequireForUpdate<LocalToWorld>();
            
            var components = new NativeArray<ComponentType>(2, Allocator.Temp);
            components[0] = ComponentType.ReadWrite<EnemyBulletSpawnData>();
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
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                
                var spawner = entityManager.GetComponentData<EnemyBulletSpawnData>(entity);
                if (spawner.BulletSpawnCurrentTime > 0f)
                {
                    spawner.BulletSpawnCurrentTime -= deltaTime;
                }
                else
                {
                    spawner.BulletSpawnCurrentTime = spawner.BulletSpawnInterval;
                    CreateBullets(ref entityManager, ref entityCommandBuffer, entity, spawner);
                }

                spawner.CurrentTime += deltaTime;
                entityManager.SetComponentData(entity, spawner);
            }

            entityCommandBuffer.Playback(entityManager);
            entities.Dispose();
            entityCommandBuffer.Dispose();
        }

        private void CreateBullets
        (
            ref EntityManager entityManager,
            ref EntityCommandBuffer entityCommandBuffer,
            Entity entity,
            EnemyBulletSpawnData spawner
        )
        {
            var spawnerLtw = entityManager.GetComponentData<LocalToWorld>(entity);
            var amplitude = spawner.BulletSpawnAngleAmplitude;
            var frequency = spawner.BulletSpawnAngleFrequency;
            var baseAngle = amplitude * math.sin(2 * math.PI * frequency * spawner.CurrentTime);

            var bulletNum = 2 * math.max(spawner.BulletSpawnWay, 1) - 1;
            var numHalf = bulletNum / 2;
            for (var i = -numHalf; i <= numHalf; i++)
            {
                var wayAngle = i * spawner.BulletSpawnAngleBetweenWay;
                var wayRotation = quaternion.Euler(0f, baseAngle + wayAngle, 0f);
                var rotation = math.mul(wayRotation, spawnerLtw.Rotation);

                var bulletEntity = entityCommandBuffer.Instantiate(spawner.BulletPrototype);
                var bulletTransform = LocalTransform.FromPositionRotation(spawnerLtw.Position, rotation);
                entityCommandBuffer.AddComponent(bulletEntity, bulletTransform);
            }
        }
    }
}