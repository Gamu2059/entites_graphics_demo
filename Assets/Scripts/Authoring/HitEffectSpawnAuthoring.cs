using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class HitEffectSpawnAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem hitEffectPrefab;

        private class Baker : Baker<HitEffectSpawnAuthoring>
        {
            public override void Bake(HitEffectSpawnAuthoring spawnAuthoring)
            {
                var hitEffectFlags = TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale;
                var hitEffectPrototype = GetEntity(spawnAuthoring.hitEffectPrefab, hitEffectFlags);

                var flags = TransformUsageFlags.Dynamic;
                var entity = GetEntity(spawnAuthoring, flags);
                if (entity == Entity.Null) return;

                var hitEffect = new HitEffectSpawnData
                {
                    HitEffectPrototype = hitEffectPrototype,
                };
                AddComponent(entity, hitEffect);
            }
        }
    }
}