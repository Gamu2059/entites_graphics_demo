using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public struct EnemyBulletSpawnData : IComponentData
    {
        public Entity BulletPrototype;
        public float BulletSpawnInterval;
        public float BulletSpawnAngleAmplitude;
        public float BulletSpawnAngleFrequency;
        public int BulletSpawnWay;
        public float BulletSpawnAngleBetweenWay;
        public float CurrentTime;
        public float BulletSpawnCurrentTime;
    }
}