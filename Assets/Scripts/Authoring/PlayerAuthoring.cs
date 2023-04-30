using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Player関連のデータをECSへ変換するためのコンポーネント.
    /// </summary>
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        /// <summary>
        /// ECSへの変換クラス.
        /// ゲーム再生時に実行される.
        /// </summary>
        private class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                // 動的に動くGameObjectであると通知して、GameObjectをEntityに変換する
                var flags = TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale;
                var entity = GetEntity(authoring, flags);
                if (entity == Entity.Null) return;

                AddComponent
                (
                    entity,
                    new PlayerData
                    {
                        MoveSpeed = authoring.moveSpeed
                    }
                );
            }
        }
    }
}