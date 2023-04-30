using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Gamu2059.EntitiesGraphicsDemo
{
    [MaterialProperty("_BaseColor")]
    public struct MaterialBaseColor : IComponentData
    {
        public float4 value;
    }
}