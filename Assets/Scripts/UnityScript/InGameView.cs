using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class InGameView : UIBehaviour
    {
        [SerializeField]
        private EnemyHpUi enemyHpUiPrefab;

        [SerializeField]
        private Camera targetCamera;

        private Dictionary<(int, int), EnemyPresentationData> _cachedEnemyDictionary;
        private Dictionary<(int, int), EnemyHpUi> _enemyHpUis;

        protected override void OnDestroy()
        {
            _enemyHpUis?.Clear();
            base.OnDestroy();
        }

        public void UpdateEnemyPresentedData(Dictionary<(int, int), EnemyPresentationData> enemyDictionary)
        {
            _cachedEnemyDictionary = enemyDictionary;
        }

        public void OnLateUpdate()
        {
            if (_cachedEnemyDictionary == null) return;

            _enemyHpUis ??= new Dictionary<(int, int), EnemyHpUi>();

            var uiKeys = _enemyHpUis.Keys;
            var dataKeys = _cachedEnemyDictionary.Keys;

            var destroyedEnemyKeys = uiKeys.Except(dataKeys).ToArray();
            if (destroyedEnemyKeys.Any())
            {
                foreach (var destroyedEnemyKey in destroyedEnemyKeys)
                {
                    _enemyHpUis.Remove(destroyedEnemyKey, out var enemyHpUi);
                    if (enemyHpUi != null)
                    {
                        Destroy(enemyHpUi.gameObject);
                    }
                }
            }

            var createdEnemyKeys = dataKeys.Except(uiKeys).ToArray();
            if (createdEnemyKeys.Any())
            {
                foreach (var createdEnemyKey in createdEnemyKeys)
                {
                    var enemyHpUi = Instantiate(enemyHpUiPrefab, transform);
                    _enemyHpUis.Add(createdEnemyKey, enemyHpUi);
                }
            }

            foreach (var enemyHpUiKeyPair in _enemyHpUis)
            {
                var data = _cachedEnemyDictionary[enemyHpUiKeyPair.Key];
                var enemyHpUi = enemyHpUiKeyPair.Value;

                var screenPos = targetCamera.WorldToScreenPoint(data.LocalToWorld.Position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    transform as RectTransform,
                    screenPos,
                    targetCamera,
                    out var localUiPos
                );

                enemyHpUi.transform.localPosition = localUiPos;
                enemyHpUi.UpdateData(data);
            }
        }
    }
}