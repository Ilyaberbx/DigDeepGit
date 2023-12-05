using _Workspace.CodeBase.Extensions;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.GemSystem
{
    public class GemsBag : MonoBehaviour
    {
        [SerializeField] private Transform _gemsRootPoint;
        private int _totalSteps;
        private int _cycleStep;

        private Vector3 RootPosition => _gemsRootPoint.localPosition;

        public void Store(Gem gem)
        {
            gem.transform.SetParent(_gemsRootPoint);
            gem.Store();

            AddStep();

            int currentStep = _totalSteps;
            int currentCycleStep = _cycleStep;

            if (EndOfCycle())
                _cycleStep = 0;

            Vector3 destination = GemDestination(gem, currentStep, currentCycleStep);

            Tweener tweener = gem.transform
                .DOLocalMove(destination, 10f)
                .SetSpeedBased(true)
                .SetAutoKill(true);

            tweener.OnUpdate(() =>
            {
                if (EnoughCloseToDestination(gem, destination))
                    tweener.ChangeEndValue(GemDestination(gem, currentStep, currentCycleStep)
                        , true);
            });
        }

        private bool EndOfCycle()
            => _cycleStep % 4 == 0;

        private bool EnoughCloseToDestination(Gem gem, Vector3 destination)
            => Vector3.Distance(gem.transform.localPosition, RootPosition + destination) > 1f;

        private void AddStep()
        {
            _totalSteps++;
            _cycleStep++;
        }

        private Vector3 GemDestination(Gem gem, int currentStep, int cycleStep) =>
            Vector3.zero
                .AddY(currentStep / 4 * gem.Size)
                .AddX(gem.Size, cycleStep % 2 == 0)
                .AddZ(gem.Size, cycleStep > 2);
    }
}