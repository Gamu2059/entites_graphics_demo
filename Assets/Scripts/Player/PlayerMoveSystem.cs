using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// Playerに入力を行うSystem.
    /// InputActionはManagedなデータのため、SystemBaseを使用.
    /// 参考 : https://qiita.com/makihiro_dev/items/e05eaa3fdf05869725ac
    /// </summary>
    public partial class PlayerMoveSystem : SystemBase
    {
        private InputActionSettings _inputActionSettings;
        private InputAction _moveInputAction;

        protected override void OnCreate()
        {
            _inputActionSettings = new InputActionSettings();
            _inputActionSettings.Enable();
            _moveInputAction = _inputActionSettings.Player.Move;
            _moveInputAction.Enable();
        }

        protected override void OnDestroy()
        {
            _moveInputAction.Disable();
            _moveInputAction.Dispose();
            _inputActionSettings.Disable();
            _inputActionSettings.Disable();
        }

        protected override void OnUpdate()
        {
            var input = _moveInputAction.ReadValue<Vector2>();

            var deltaTime = SystemAPI.Time.DeltaTime;
            var movement = new float3(input.x, 0f, input.y) * deltaTime;

            Entities
                .ForEach
                (
                    (PlayerParam playerParam, ref LocalTransform transform) =>
                    {
                        transform.Position += playerParam.MoveSpeed * movement;
                    }
                )
                .WithBurst()
                .ScheduleParallel();
        }
    }
}