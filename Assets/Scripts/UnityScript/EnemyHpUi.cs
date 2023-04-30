using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class EnemyHpUi : UIBehaviour
    {
        [SerializeField]
        private Slider hpSlider;

        public void UpdateData(EnemyPresentationData data)
        {
            float maxHp = data.EnemyData.MaxHp;
            float currentHp = data.EnemyData.CurrentHp;
            if (maxHp <= 0f)
            {
                hpSlider.value = 0f;
                return;
            }

            hpSlider.value = currentHp / maxHp;
        }
    }
}