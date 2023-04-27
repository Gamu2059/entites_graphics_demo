//  CameraSizeUpdater.cs
//  http://kan-kikuchi.hatenablog.com/entry/CameraSizeUpdater
//
//  Created by kan.kikuchi on 2019.07.02.

using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// カメラのOrthographicSizeをアス比に応じて更新するクラス
    /// </summary>
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class CameraSizeUpdater : MonoBehaviour
    {
        private Camera _camera;

        /// <summary>
        /// 縦、横、もしくは両方のどれを基準にするか
        /// </summary>
        private enum BaseType
        {
            Both,
            Width,
            Height
        }

        [SerializeField]
        private BaseType baseType = BaseType.Both;

        [SerializeField]
        private float baseWidth = 1280;

        [SerializeField]
        private float baseHeight = 1920;

        [SerializeField]
        private float pixelPerUnit = 100f;

        [SerializeField]
        private bool isAlwaysUpdate = false;

        private float _currentAspect;

        private void Awake()
        {
            UpdateOrthographicSize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _currentAspect = 0;
            UpdateOrthographicSize();
        }
#endif

        private void Update()
        {
            if (!isAlwaysUpdate && Application.isPlaying)
            {
                return;
            }

            UpdateOrthographicSize();
        }

        /// <summary>
        /// カメラのOrthographicSizeをアス比に応じて更新
        /// </summary>
        private void UpdateOrthographicSize()
        {
            // 現在のアスペクト比を取得し、変化がなければ更新しない
            var currentAspect = (float)Screen.height / Screen.width;
            if (Mathf.Approximately(_currentAspect, currentAspect)) return;

            _currentAspect = currentAspect;

            // カメラを取得していなければ取得
            if (_camera == null)
            {
                _camera = gameObject.GetComponent<Camera>();
            }

            // 基準のアスペクト比と、基準のアスペクト比の時のSize
            var baseAspect = baseHeight / baseWidth;
            var baseOrthographicSize = baseHeight / pixelPerUnit / 2f;

            // カメラのorthographicSizeを設定しなおす
            if (baseType == BaseType.Height || (baseAspect > _currentAspect && baseType != BaseType.Width))
            {
                _camera.orthographicSize = baseOrthographicSize;
            }
            else
            {
                _camera.orthographicSize = baseOrthographicSize * (_currentAspect / baseAspect);
            }
        }
    }
}