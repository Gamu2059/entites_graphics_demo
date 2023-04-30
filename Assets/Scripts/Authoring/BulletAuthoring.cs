using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// 弾丸関連のデータをECSへ変換するためのコンポーネント.
    /// </summary>
    public class BulletAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private int damage;

        [SerializeField]
        private float autoDestroyDistance;

        private class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                // 動的に動くGameObjectであると通知して、GameObjectをEntityに変換する
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                if (entity == Entity.Null) return;

                AddComponent
                (
                    entity,
                    new BulletData
                    {
                        Speed = authoring.speed,
                        Damage = authoring.damage,
                        AutoDestroyDistance = authoring.autoDestroyDistance
                    }
                );
            }
        }
    }
}