using Unity.Entities;
using Unity.Mathematics;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct EnemyBody : IComponentData
    {
        public bool WasSearchEnemyDataEntity;
        public Entity EnemyDataEntity;

        public float4 originalBodyBaseColor;
    }
}