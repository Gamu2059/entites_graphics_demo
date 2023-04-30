using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class EnemyBodyAuthoring : MonoBehaviour
    {
        private class Baker : Baker<EnemyBodyAuthoring>
        {
            public override void Bake(EnemyBodyAuthoring authoring)
            {
                // 元のBaseColorを設定する
                var color = Color.white;
                if (authoring.TryGetComponent<Renderer>(out var renderer))
                {
                    var id = Shader.PropertyToID("_BaseColor");
                    color = renderer.sharedMaterial.GetColor(id);
                }

                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyBody { originalBodyBaseColor = (Vector4)color });
            }
        }
    }
}