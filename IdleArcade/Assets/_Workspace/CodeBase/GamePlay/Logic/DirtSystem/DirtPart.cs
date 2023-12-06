using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DirtPart : MonoBehaviour
    {
        [SerializeField] private float _digDuration;

        private Vector3 _cachedLocalScale;
        private Vector3 _cachedLocalPosition;
        private Quaternion _cachedLocalRotation;

        private Transform _cachedTransform;

        public void Initialize()
            => CacheStartValues();

        public async UniTask Dig()
        {
            List<UniTask> tasks = new List<UniTask>();

            tasks.Add(_cachedTransform.DOLocalRotate(Vector3.one * Random.Range(-360, 360), _digDuration, RotateMode.FastBeyond360)
                .ToUniTask());

            tasks.Add(_cachedTransform.DOLocalMoveY(transform.position.y + 5f, _digDuration)
                .ToUniTask());

            tasks.Add(_cachedTransform.DOScale(Vector3.zero, _digDuration)
                .ToUniTask());

            await UniTask.WhenAll(tasks);

            ResetValues();
        }

        private void ResetValues()
        {
            _cachedTransform.localScale = _cachedLocalScale;
            _cachedTransform.localPosition = _cachedLocalPosition;
            _cachedTransform.localRotation = _cachedLocalRotation;
        }

        private void CacheStartValues()
        {
            _cachedTransform = transform;

            _cachedLocalScale = _cachedTransform.localScale;
            _cachedLocalPosition = _cachedTransform.localPosition;
            _cachedLocalRotation = _cachedTransform.localRotation;
        }
    }
}