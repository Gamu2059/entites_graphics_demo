using System;
using Unity.Collections;
using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class ParticleCountSystem : SystemBase
    {
        public static event Action<int> OnCountedParticleNum;

        private EntityQuery _entityQuery;

        protected override void OnCreate()
        {
            _entityQuery = GetEntityQuery(ComponentType.ReadOnly<HitEffectPlayingData>());
        }

        protected override void OnUpdate()
        {
            var entities = _entityQuery.ToEntityArray(Allocator.Temp);
            OnCountedParticleNum?.Invoke(entities.Length);
            entities.Dispose();
        }
    }
}