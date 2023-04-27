using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Enemyということを示すためだけのデータ.
    /// </summary>
    public struct EnemyTag : IComponentData
    {
    }

    /// <summary>
    /// Enemyのパラメータデータ.
    /// </summary>
    public struct EnemyParam : IComponentData
    {
        // 振幅
        public float Amplitude;

        // 周波数
        public float Frequency;

        // 元々いた座標
        public float3 OriginPosition;

        // 現在の保持している時間 (保持用)
        public float CurrentTime;
    }

    /// <summary>
    /// Enemy関連のデータをECSへ変換するためのコンポーネント.
    /// </summary>
    public class EnemyAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float amplitude;

        [SerializeField]
        private float frequency;

        private class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                // 動的に動くGameObjectであると通知して、GameObjectをEntityに変換する
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                if (entity == Entity.Null) return;

                AddComponent(entity, new EnemyTag());
                AddComponent
                (
                    entity,
                    new EnemyParam
                    {
                        Amplitude = authoring.amplitude,
                        Frequency = authoring.frequency,
                        OriginPosition = authoring.transform.position
                    }
                );
            }
        }
    }
}