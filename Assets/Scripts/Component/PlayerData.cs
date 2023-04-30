using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Playerのパラメータデータ.
    /// </summary>
    public struct PlayerData : IComponentData
    {
        public float MoveSpeed;
    }
}