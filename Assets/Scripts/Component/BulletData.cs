using Unity.Entities;
using Unity.Mathematics;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct BulletData : IComponentData
    {
        public float Speed;
        public int Damage;
        public float AutoDestroyDistance;
        public float3 PrePosition;
        public float3 CurrentPosition;
    }
}