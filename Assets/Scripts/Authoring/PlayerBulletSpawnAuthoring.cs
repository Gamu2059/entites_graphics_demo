using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class PlayerBulletSpawnAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private float spawnInterval;

        private class Baker : Baker<PlayerBulletSpawnAuthoring>
        {
            public override void Bake(PlayerBulletSpawnAuthoring authoring)
            {
                var bulletFlags = TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale;
                var bulletPrototype = GetEntity(authoring.bulletPrefab, bulletFlags);

                var flags = TransformUsageFlags.Dynamic;
                var entity = GetEntity(authoring, flags);
                var data = new PlayerBulletSpawnData
                {
                    BulletPrototype = bulletPrototype,
                    BulletSpawnInterval = authoring.spawnInterval,
                    BulletSpawnCurrentTime = 0f,
                };
                AddComponent(entity, data);
            }
        }
    }
}