using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [RequireComponent(typeof(Renderer))]
    public class CharaBodyAuthoring : MonoBehaviour
    {
        private class Baker : Baker<CharaBodyAuthoring>
        {
            public override void Bake(CharaBodyAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharaBodyData { BodyEntity = entity });
            }
        }
    }
}