using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Playerの入力を行うSystem.
    /// InputActionはManagedなデータのため、SystemBaseを使用.
    /// 参考 : https://qiita.com/makihiro_dev/items/e05eaa3fdf05869725ac
    /// </summary>
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class PlayerInputSystem : SystemBase
    {
        private bool _initialized;
        private InputActionSettings _inputActionSettings;
        private InputAction _moveInputAction;

        protected override void OnCreate()
        {
            _initialized = false;
            if (SystemAPI.HasSingleton<PlayerInputData>()) return;

            _initialized = true;
            _inputActionSettings = new InputActionSettings();
            _inputActionSettings.Enable();
            _moveInputAction = _inputActionSettings.Player.Move;
            _moveInputAction.Enable();

            EntityManager.CreateSingleton(new PlayerInputData());
        }

        protected override void OnDestroy()
        {
            if (!_initialized) return;

            var entity = SystemAPI.GetSingletonEntity<PlayerInputData>();
            EntityManager.DestroyEntity(entity);

            _moveInputAction.Disable();
            _moveInputAction.Dispose();
            _inputActionSettings.Disable();
            _inputActionSettings.Disable();
        }

        protected override void OnUpdate()
        {
            if (!_initialized) return;
            if (!SystemAPI.HasSingleton<PlayerInputData>()) return;

            var playerInputData = SystemAPI.GetSingleton<PlayerInputData>();
            playerInputData.Move = _moveInputAction.ReadValue<Vector2>();
            SystemAPI.SetSingleton(playerInputData);
        }
    }
}