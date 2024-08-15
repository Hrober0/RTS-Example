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

        [SerializeField] private Agent _agentPrefab;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Transform _agentMovementSpaceMin;
        [SerializeField] private Transform _agentMovementSpaceMax;

        private readonly List<Agent> _acticveAgents = new();
        private ObjectPool<Agent> _pool;

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

            yield return null;
        }

        void IService.Deinit()
        {
            
        }

        public void SpawnAgent()
        {
            var agent = _pool.Get();
            agent.transform.position = GetRandomMapPosition();
            agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            _acticveAgents.Add(agent);
            OnNumberOfAgentsChanged?.Invoke();
        }

        public void KillAgent()
        {
            if (_acticveAgents.Count <= 0)
            {
                Debug.LogWarning("No agent no kill");
                return;
            }

            var randomIndex = Random.Range(0, _acticveAgents.Count);
            var agent = _acticveAgents[randomIndex];
            _acticveAgents.RemoveAt(randomIndex);

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