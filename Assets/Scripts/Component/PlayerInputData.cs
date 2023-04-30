using Unity.Entities;
using Unity.Mathematics;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct PlayerInputData : IComponentData
    {
        public float2 Move;
    }
}