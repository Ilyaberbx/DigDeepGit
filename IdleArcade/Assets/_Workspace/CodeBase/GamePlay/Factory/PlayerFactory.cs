using System.Threading.Tasks;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.Service.Factory;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Factory
{
    public class PlayerFactory
    {
        private readonly IPrefabFactoryAsync _prefabFactory;

        public PlayerFactory(IPrefabFactoryAsync prefabFactory) 
            => _prefabFactory = prefabFactory;

        public async Task<GameObject> CreatePlayer(Vector3 at) 
            => await _prefabFactory.Create<GameObject>(GamePlayAssetsAddress.Player,at,null);
    }
}