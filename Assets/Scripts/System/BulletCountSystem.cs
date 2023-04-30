using System;
using Unity.Collections;
using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class BulletCountSystem : SystemBase
    {
        public static event Action<int> OnCountedBulletNum;

        private EntityQuery _entityQuery;

        protected override void OnCreate()
        {
            _entityQuery = GetEntityQuery(ComponentType.ReadOnly<BulletData>());
        }

        protected override void OnUpdate()
        {
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            OnCountedBulletNum?.Invoke(entities.Length);
            entities.Dispose();
        }
    }
}