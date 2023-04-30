using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class EnemyBulletSpawnAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private float spawnInterval;

        [SerializeField]
        private float spawnAngleAmplitude;

        [SerializeField]
        private float spawnAngleFrequency;
        
        [SerializeField]
        private int spawnWay;
        
        [SerializeField]
        private float spawnAngleBetweenWay;

        private class Baker : Baker<EnemyBulletSpawnAuthoring>
        {
            public override void Bake(EnemyBulletSpawnAuthoring authoring)
            {
                var bulletFlags = TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale;
                var bulletPrototype = GetEntity(authoring.bulletPrefab, bulletFlags);

                var flags = TransformUsageFlags.Dynamic;
                var entity = GetEntity(authoring, flags);
                var data = new EnemyBulletSpawnData
                {
                    BulletPrototype = bulletPrototype,
                    BulletSpawnInterval = authoring.spawnInterval,
                    BulletSpawnAngleAmplitude = authoring.spawnAngleAmplitude,
                    BulletSpawnAngleFrequency = authoring.spawnAngleFrequency,
                    BulletSpawnWay = authoring.spawnWay,
                    BulletSpawnAngleBetweenWay = authoring.spawnAngleBetweenWay,
                    CurrentTime = 0f,
                    BulletSpawnCurrentTime = 0f,
                };
                AddComponent(entity, data);
            }
        }
    }
}