using Unity.Entities;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Enemy関連のデータをECSへ変換するためのコンポーネント.
    /// </summary>
    public class EnemyAuthoring : MonoBehaviour
    {
        [SerializeField]
        private int maxHp;
        
        [SerializeField]
        private float amplitude;

        [SerializeField]
        private float frequency;

        [SerializeField]
        private float offsetPhase;

        private class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                var enemyData = new EnemyData
                {
                    MaxHp = authoring.maxHp,
                    CurrentHp = authoring.maxHp,
                    Amplitude = authoring.amplitude,
                    Frequency = authoring.frequency,
                    OffsetPhase = authoring.offsetPhase,
                    OriginPosition = authoring.transform.position,
                    CurrentTime = 0f,
                };
                AddComponent(entity, enemyData);

            }
        }
    }
}