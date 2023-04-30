using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// ヒットエフェクト自身に持たせる再生パラメータ
    /// </summary>
    public struct HitEffectPlayingData : IComponentData
    {
        public float AutoDestroyTime;
    }
}