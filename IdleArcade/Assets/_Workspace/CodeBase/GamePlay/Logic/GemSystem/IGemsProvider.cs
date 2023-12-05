using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;

namespace _Workspace.CodeBase.GamePlay.Logic.GemSystem
{
    public interface IGemsProvider
    {
        Dictionary<Dirt, int> GetGemsByDirtMap(List<Dirt> dirtList, int totalGems);
    }
}