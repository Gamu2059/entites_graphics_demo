using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct PlayerBulletSpawnData : IComponentData
    {
        public Entity BulletPrototype;
        public float BulletSpawnInterval;
        public float BulletSpawnCurrentTime;
    }
}