using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using DG.Tweening;
using Pathfinding;

namespace Agents
{
    public class Agent : MonoBehaviour
    {
        public event Action<Agent> OnReachedTarget;
        public event Action OnMovementSpeedChanged;

        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _rotationTime = 0.3f;
        [SerializeField] private Seeker _seeker;

        private IAgentService _agentService;
        private Path _path;
        private int _pathIndex;

        private float _speedMultiplier = 1f;
        private Tween _movementTween;

        public int Guid { get; private set; }
        public float MovementSpeed => _movementSpeed * _speedMultiplier;

        private void Start()
        {
            _seeker.pathCallback = SetNewPath;
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void Init(int guid)
        {
            Guid = guid;
            _agentService = ServiceManager.Instance.Get<IAgentService>();

            RequestNewPah();
        }

        public void ChangeSpeed(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
            OnMovementSpeedChanged?.Invoke();
            if (_movementTween != null)
            {
                _movementTween.timeScale = _speedMultiplier;
            }
        }

        private void RequestNewPah()
        {
            var target = _agentService.GetRandomMapPosition();
            _seeker.StartPath(transform.position, target);
        }

        private void SetNewPath(Path path)
        {
            _path = path;
            _pathIndex = 0;
            OnPathPointReached();
        }

        private void GoToPathPoint()
        {
            var pathPoint = (Vector3)_path.path[_pathIndex].position;
            var direction = pathPoint - transform.position;
            var distance = direction.magnitude;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, _rotationTime)
                .SetEase(Ease.InQuad)
                .timeScale = _speedMultiplier;
            _movementTween = transform.DOMove(pathPoint, distance / _movementSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(OnPathPointReached);
            _movementTween.timeScale = _speedMultiplier;
        }

        private void OnPathPointReached()
        {
            _pathIndex++;
            if (_pathIndex < _path.path.Count)
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
            OnReachedTarget?.Invoke(this);
            RequestNewPah();
        }
    }
}