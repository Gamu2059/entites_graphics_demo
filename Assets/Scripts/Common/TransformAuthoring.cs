using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class TransformAuthoring : MonoBehaviour
    {
        private class Baker : Baker<Transform>
        {
            public override void Bake(Transform authoring)
            {
                // 動的に動くGameObjectであると通知して、GameObjectをEntityに変換する
                var flags = TransformUsageFlags.NonUniformScale | TransformUsageFlags.Dynamic;
                var entity = GetEntity(authoring, flags);
                if (entity == Entity.Null) return;

                AddComponent
                (
                    entity,
                    new LocalToWorld
                    {
                        Value = authoring.localToWorldMatrix
                    }
                );
            }
        }
    }
}