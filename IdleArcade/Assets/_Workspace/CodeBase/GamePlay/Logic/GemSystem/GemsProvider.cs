using System.Collections.Generic;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.GemSystem
{
    public class GemsProvider : IGemsProvider
    {
        private const int MinGems = 20;
        private const int MaxGems = 40;

        public Dictionary<Dirt, int> GetGemsByDirtMap(List<Dirt> dirtList, int depth)
        {
            int totalGems = GetRandomGemsByDepth(depth);
            Dictionary<Dirt, int> dirtGemsMap = new Dictionary<Dirt, int>();

            dirtList.Shuffle();

            int dirtWithGems = Random.Range(dirtList.Count / 2, dirtList.Count + 1);
            int averageGemsPerDirt = totalGems / dirtWithGems;
            int remainingGems = totalGems % dirtWithGems;

            for (int i = 0; i < dirtList.Count; i++)
            {
                if (dirtWithGems > i)
                {
                    dirtGemsMap.Add(dirtList[i], averageGemsPerDirt);

                    if (remainingGems > 0)
                    {
                        int extraGems = Random.Range(0, 2);
                        dirtGemsMap[dirtList[i]] += extraGems;
                        remainingGems -= extraGems;
                    }
                }
                else
                    dirtGemsMap.Add(dirtList[i], 0);
            }

            return dirtGemsMap;
        }

        private int GetRandomGemsByDepth(int depth)
            => Random.Range(MinGems, MaxGems + 1) + 2 * depth;
    }
}