using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct EnemyPresentationData
    {
        public Entity Entity;
        public LocalToWorld LocalToWorld;
        public EnemyData EnemyData;
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class EnemyPresentationSystem : SystemBase
    {
        public static event Action<EnemyPresentationData[]> OnPresentedEnemies;

        private EntityQuery _entityQuery;

        protected override void OnCreate()
        {
            _entityQuery = GetEntityQuery
            (
                ComponentType.ReadOnly<LocalToWorld>(),
                ComponentType.ReadOnly<EnemyData>()
            );
        }

        protected override void OnUpdate()
        {
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            var array = new EnemyPresentationData[entities.Length];
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                array[i] = new EnemyPresentationData
                {
                    Entity = entity,
                    LocalToWorld = EntityManager.GetComponentData<LocalToWorld>(entity),
                    EnemyData = EntityManager.GetComponentData<EnemyData>(entity),
                };
            }

            entities.Dispose();
            OnPresentedEnemies?.Invoke(array);
        }
    }
}