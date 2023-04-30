using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [BurstCompile]
    // 物理挙動の処理なのでFixedStepSimulationSystemGroup下で実行する
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    // 物理挙動のシステムの後に実行する
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct BulletTriggerEventSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _localTransformLookup;
        private ComponentLookup<BulletData> _bulletLookup;
        private ComponentLookup<EnemyData> _enemyLookup;
        private ComponentLookup<HitEffectSpawnData> _hitEffectSpawnLookup;
        private ComponentLookup<HitEffectInitData> _hitEffectInitLookup;

        [BurstCompile]
        void ISystem.OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<LocalTransform>();
            state.RequireForUpdate<BulletData>();

            _localTransformLookup = state.GetComponentLookup<LocalTransform>();
            _bulletLookup = state.GetComponentLookup<BulletData>();
            _enemyLookup = state.GetComponentLookup<EnemyData>();
            _hitEffectSpawnLookup = state.GetComponentLookup<HitEffectSpawnData>();
            _hitEffectInitLookup = state.GetComponentLookup<HitEffectInitData>();
        }

        [BurstCompile]
        void ISystem.OnUpdate(ref SystemState state)
        {
            _localTransformLookup.Update(ref state);
            _bulletLookup.Update(ref state);
            _enemyLookup.Update(ref state);
            _hitEffectSpawnLookup.Update(ref state);
            _hitEffectInitLookup.Update(ref state);

            var entityManager = state.EntityManager;
            var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.MultiPlayback);
            new BulletTriggerEventJob
                {
                    LocalTransformLookup = _localTransformLookup,
                    BulletLookup = _bulletLookup,
                    EnemyLookup = _enemyLookup,
                    HitEffectSpawnDataLookup = _hitEffectSpawnLookup,
                    HitEffectInitDataLookup = _hitEffectInitLookup,
                    EntityManager = entityManager,
                    EntityCommandBuffer = entityCommandBuffer
                }
                .Schedule(simulationSingleton, state.Dependency)
                .Complete();

            entityCommandBuffer.Playback(entityManager);
            entityCommandBuffer.Dispose();
        }
    }

    /// <summary>
    /// 弾の衝突判定を処理するJob.
    /// </summary>
    [BurstCompile]
    public struct BulletTriggerEventJob : ITriggerEventsJob
    {
        [ReadOnly]
        public ComponentLookup<LocalTransform> LocalTransformLookup;

        [ReadOnly]
        public ComponentLookup<BulletData> BulletLookup;

        [ReadOnly]
        public ComponentLookup<EnemyData> EnemyLookup;

        [ReadOnly]
        public ComponentLookup<HitEffectSpawnData> HitEffectSpawnDataLookup;

        [ReadOnly]
        public ComponentLookup<HitEffectInitData> HitEffectInitDataLookup;

        public EntityManager EntityManager;
        public EntityCommandBuffer EntityCommandBuffer;

        [BurstCompile]
        public void Execute(TriggerEvent collisionEvent)
        {
            // 衝突した2つのEntityを取り出す
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;

            // 2つのうちどちらがBulletDataかを調べる
            var isBulletA = BulletLookup.HasComponent(entityA);
            var isBulletB = BulletLookup.HasComponent(entityB);
            if (isBulletA == isBulletB) return;

            var bulletEntity = isBulletA ? entityA : entityB;
            var otherEntity = isBulletA ? entityB : entityA;
            CreateHitEffect(bulletEntity, otherEntity);
            
            // 被弾したのが敵だった場合、ヒットしたという情報を書く
            if (EnemyLookup.TryGetComponent(otherEntity, out var enemyData))
            {
                var bulletData = BulletLookup[bulletEntity];
                enemyData.CurrentHp -= bulletData.Damage;
                
                // 被弾した敵は0.1秒その状態が続く
                enemyData.HitValidRemainingTime = 0.1f;
                EntityCommandBuffer.SetComponent(otherEntity, enemyData);
                
                // Hp0なら敵を削除する
                if (enemyData.CurrentHp <= 0)
                {
                    EntityManager.DestroyEntityRecursive(EntityCommandBuffer, otherEntity);
                }
            }
        }

        [BurstCompile]
        private void CreateHitEffect(Entity bulletEntity, Entity otherEntity)
        {
            // 被弾した側がヒットエフェクトスポナーを持っていればヒットエフェクトを再生する
            if (!HitEffectSpawnDataLookup.TryGetComponent(otherEntity, out var hitEffect)) return;

            var effectEntity = EntityCommandBuffer.Instantiate(hitEffect.HitEffectPrototype);
            var bulletTransform = LocalTransformLookup[bulletEntity];
            var effectRotation = quaternion.LookRotation(-bulletTransform.Forward(), new float3(0f, 1f, 0f));
            var effectTransform = LocalTransform.FromPositionRotation(bulletTransform.Position, effectRotation);
            EntityCommandBuffer.AddComponent(effectEntity, effectTransform);

            HitEffectPlayingData effectPlayingData = new HitEffectPlayingData
            {
                AutoDestroyTime = 1f,
            };

            // ヒットエフェクトが初期パラメータを持っていれば再生パラメータに入れる
            if (HitEffectInitDataLookup.TryGetComponent(hitEffect.HitEffectPrototype, out var hitInitData))
            {
                effectPlayingData.AutoDestroyTime = hitInitData.AutoDestroyTime;
            }

            EntityCommandBuffer.AddComponent(effectEntity, effectPlayingData);
            EntityCommandBuffer.DestroyEntity(bulletEntity);
        }
    }
}