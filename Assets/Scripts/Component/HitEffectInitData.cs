using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// ヒットエフェクト自身に持たせている初期パラメータ
    /// </summary>
    public struct HitEffectInitData : IComponentData
    {
        public float AutoDestroyTime;
    }
}