using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using Core;
using DG.Tweening;

namespace Agents
{
    public class Agent : MonoBehaviour
    {
        public event Action OnReachedTarget;

        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _rotationTime = 0.3f;

        private IPathFindingService _pathFindingService;
        private IAgentService _agentService;
        private Vector3[] _path;
        private int _pathIndex;

        public int Guid { get; private set; }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void Init(int guid)
        {
            Guid = guid;
            _pathFindingService = ServiceManager.Instance.Get<IPathFindingService>();
            _agentService = ServiceManager.Instance.Get<IAgentService>();

            SetNewPath();
        }

        private void SetNewPath()
        {
            var target = _agentService.GetRandomMapPosition();
            _path = _pathFindingService.GetPathToPoint(target);
            _pathIndex = 0;
            GoToPathPoint();
        }

        private void GoToPathPoint()
        {
            var pathPoint = _path[_pathIndex];
            var direction = pathPoint - transform.position;
            var distance = direction.magnitude;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, _rotationTime)
                .SetEase(Ease.InQuad);
            transform.DOMove(pathPoint, distance / _movementSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(OnPathPointReached);
        }

        private void OnPathPointReached()
        {
            _pathIndex++;
            if (_pathIndex < _path.Length)
            {
                GoToPathPoint();
            }
            else
            {
                ReachedTarget();
            }
        }

        private void ReachedTarget()
        {
            OnReachedTarget?.Invoke();
            SetNewPath();
        }
    }
}