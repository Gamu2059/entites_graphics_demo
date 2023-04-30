using Unity.Entities;
using Unity.Mathematics;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Enemyのパラメータデータ.
    /// </summary>
    public struct EnemyData : IComponentData
    {
        public int MaxHp;
        public int CurrentHp;
        
        public float Amplitude;
        public float Frequency;
        public float OffsetPhase;
        public float3 OriginPosition;
        public float CurrentTime;

        public float HitValidRemainingTime;
        public bool IsValidHit => HitValidRemainingTime > 0f;
    }
}