using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.UI.Player.Handler;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.GemSystem
{
    public class GemsBag : MonoBehaviour
    {
        [SerializeField] private Transform _gemsRootPoint;
        private int _totalGems;
        private int _cycleStep;
        private IEventBusService _eventBus;

        [Inject]
        public void Construct(IEventBusService eventBus) 
            => _eventBus = eventBus;
        
        private Vector3 RootPosition => _gemsRootPoint.localPosition;

        public void Store(Gem gem)
        {
            gem.transform.SetParent(_gemsRootPoint);
            gem.Store();

            AddStep();
            InformAddGem();

            int currentStep = _totalGems;
            int currentCycleStep = _cycleStep;

            if (EndOfCycle())
                _cycleStep = 0;

            Vector3 destination = GemDestination(gem, currentStep, currentCycleStep);

            Tweener tweener = gem.transform
                .DOLocalMove(destination, 3f / DistanceToGemDestination(gem, destination))
                .SetAutoKill(true);

            tweener.OnUpdate(() =>
            {
                if (EnoughCloseToDestination(gem, destination))
                    tweener.ChangeEndValue(GemDestination(gem, currentStep, currentCycleStep)
                        , true);
            });
        }

        private void InformAddGem() 
            => _eventBus
                .RaiseEvent<IGemsCountUpdateHandler>(handler => handler
                    .HandleGemsCountUpdate(_totalGems));

        private bool EndOfCycle()
            => _cycleStep % 4 == 0;

        private bool EnoughCloseToDestination(Gem gem, Vector3 destination)
            => DistanceToGemDestination(gem, destination) > 0.05f;

        private float DistanceToGemDestination(Gem gem, Vector3 destination)
            => Vector3.Distance(gem.transform.localPosition, RootPosition + destination);

        private void AddStep()
        {
            _totalGems++;
            _cycleStep++;
        }

        private Vector3 GemDestination(Gem gem, int currentStep, int cycleStep) =>
            Vector3.zero
                .AddY(Mathf.RoundToInt(currentStep / 4f) * gem.Size)
                .AddX(gem.Size, cycleStep % 2 == 0)
                .AddZ(gem.Size, cycleStep > 2);
    }
}