using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Agents
{
    public class AgentsManager : MonoBehaviour, IAgentService
    {
        public event Action OnNumberOfAgentsChanged;
        public event Action<INotification> OnNotification;

        [SerializeField] private Agent _agentPrefab;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Transform _agentMovementSpaceMin;
        [SerializeField] private Transform _agentMovementSpaceMax;

        private readonly List<Agent> _acticveAgents = new();
        private ObjectPool<Agent> _pool;
        private int _nextAgentGUID = 0;
        private ITickService _tickService;

        public int NumberOfAgents => _acticveAgents.Count;

        IEnumerator IService.Init()
        {
            _pool = new(
                createFunc: () => Instantiate(_agentPrefab),
                actionOnGet: (agent) => agent.gameObject.SetActive(true),
                actionOnRelease: (agent) => agent.gameObject.SetActive(false),
                actionOnDestroy: (agent) => Destroy(agent.gameObject),
                defaultCapacity: 100
                );

            _tickService = ServiceManager.Instance.Get<ITickService>();
            _tickService.OnGameSpeedChanged += UpdateSpeedOfAgents;
            _tickService.OnPauseStateChanged += UpdateSpeedOfAgents;

            yield return null;
        }

        void IService.Deinit()
        {
            _tickService.OnGameSpeedChanged -= UpdateSpeedOfAgents;
            _tickService.OnPauseStateChanged -= UpdateSpeedOfAgents;
        }

        public void RequestAgentSpawn()
        {
            var agent = _pool.Get();
            agent.transform.position = GetRandomMapPosition();
            agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            agent.Init(_nextAgentGUID++);
            agent.ChangeSpeed(_tickService.IsPaused ? 0 : _tickService.GameSpeed);
            agent.OnReachedTarget += SendAgentReachedNotyfication;
            _acticveAgents.Add(agent);
            OnNumberOfAgentsChanged?.Invoke();
        }

        public void RequestAgentKill()
        {
            if (_acticveAgents.Count <= 0)
            {
                Debug.LogWarning("No agent no kill");
                return;
            }

            var randomIndex = Random.Range(0, _acticveAgents.Count);
            var agent = _acticveAgents[randomIndex];
            _acticveAgents.RemoveAt(randomIndex);

            agent.OnReachedTarget -= SendAgentReachedNotyfication;

            _pool.Release(agent);

            OnNumberOfAgentsChanged?.Invoke();
        }

        public Vector3 GetRandomMapPosition()
        {
            for (int i = 0; i < 100; i++)
            {
                if (TryGetRandomMapPosition(out var position))
                {
                    return position;
                }
            }

            Debug.LogError($"Ground not found");
            return default;
        }

        private bool TryGetRandomMapPosition(out Vector3 position)
        {
            var min = _agentMovementSpaceMin.position;
            var max = _agentMovementSpaceMax.position;

            var spawnPosition = new Vector3(
                Random.Range(min.x, max.x),
                max.y,
                Random.Range(min.z, max.z));

            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, (max.y - min.y), _groundMask))
            {
                position = hit.point;
                return true;
            }

            position = default;
            return false;
        }

        private void UpdateSpeedOfAgents()
        {
            var speed = _tickService.IsPaused ? 0 : _tickService.GameSpeed;
            foreach (var agent in _acticveAgents)
            {
                agent.ChangeSpeed(speed);
            }
        }

        private void SendAgentReachedNotyfication(Agent agent)
        {
            OnNotification?.Invoke(new AgentReachedTargetNotification(agent));
        }

        private void OnDrawGizmosSelected()
        {
            if (_agentMovementSpaceMin != null && _agentMovementSpaceMax != null)
            {
                var min = _agentMovementSpaceMin.position;
                var max = _agentMovementSpaceMax.position;
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube((max + min) / 2f, max - min);
            }
        }
    }
}