using System;
using System.Collections.Generic;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class InGameModel : IDisposable
    {
        public event Action<Dictionary<(int, int), EnemyPresentationData>> OnUpdateEnemyPresentedData;

        public InGameModel()
        {
            EnemyPresentationSystem.OnPresentedEnemies += OnPresentedEnemies;
        }

        public void Dispose()
        {
            EnemyPresentationSystem.OnPresentedEnemies -= OnPresentedEnemies;
        }

        private void OnPresentedEnemies(EnemyPresentationData[] dataArray)
        {
            // 要素数が0もあり得る
            if (dataArray == null) return;

            var currentEnemyDataDictionary = new Dictionary<(int, int), EnemyPresentationData>();
            foreach (var data in dataArray)
            {
                var id = (data.Entity.Index, data.Entity.Version);
                currentEnemyDataDictionary.Add(id, data);
            }

            OnUpdateEnemyPresentedData?.Invoke(currentEnemyDataDictionary);
        }
    }
}