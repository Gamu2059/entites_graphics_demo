using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class HitEffectInitAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float autoDestroyTime;

        private class Baker : Baker<HitEffectInitAuthoring>
        {
            public override void Bake(HitEffectInitAuthoring authoring)
            {
                var flags = TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale;
                var entity = GetEntity(authoring, flags);
                if (entity == Entity.Null) return;

                AddComponent(entity, new HitEffectInitData { AutoDestroyTime = authoring.autoDestroyTime });
            }
        }
    }
}