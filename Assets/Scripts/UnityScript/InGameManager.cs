using System;
using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField]
        private InGameView view;
        
        private InGameModel _model;
        
        private void Awake()
        {
            _model = new InGameModel();
            _model.OnUpdateEnemyPresentedData += view.UpdateEnemyPresentedData;
        }

        private void OnDestroy()
        {
            _model.OnUpdateEnemyPresentedData -= view.UpdateEnemyPresentedData;
            _model?.Dispose();
        }

        private void LateUpdate()
        {
            view.OnLateUpdate();
        }
    }
}