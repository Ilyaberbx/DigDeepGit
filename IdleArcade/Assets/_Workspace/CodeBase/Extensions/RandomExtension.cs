using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace _Workspace.CodeBase.Extensions
{
    public static class RandomExtension
    {
        
        public static void Shuffle<T>(this List<T> list)
        {
            int listCount = list.Count;
            for (int i = 0; i < listCount; i++)
            {
                T oldVal = list[i];
                int randomIndex = Random.Range(i, listCount);
                list[i] = list[randomIndex];
                list[randomIndex] = oldVal;
            }
        }
    }
}