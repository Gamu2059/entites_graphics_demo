using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class MaterialBaseColorAuthoring : MonoBehaviour
    {
        private class Baker : Baker<MaterialBaseColorAuthoring>
        {
            public override void Bake(MaterialBaseColorAuthoring authoring)
            {
                var bodyEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                var bodyColor = Color.red.linear;
                var data = new MaterialBaseColor { value = (Vector4)bodyColor };
                AddComponent(bodyEntity, data);
            }
        }
    }
}