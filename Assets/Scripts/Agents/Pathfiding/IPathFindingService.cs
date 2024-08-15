using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public interface IPathFindingService : IService
    {
        Vector3[] GetPathToPoint(Vector3 target);
    }
}