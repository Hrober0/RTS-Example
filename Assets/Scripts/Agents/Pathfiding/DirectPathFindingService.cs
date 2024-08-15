using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class DirectPathFindingService : MonoBehaviour, IPathFindingService
    {
        IEnumerator IService.Init()
        {
            yield return null;
        }

        void IService.Deinit()
        {

        }

        public Vector3[] GetPathToPoint(Vector3 target)
        {
            return new Vector3[] { target };
        }
    }
}