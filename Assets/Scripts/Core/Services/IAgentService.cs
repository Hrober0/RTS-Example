using System;
using UnityEngine;

namespace Core
{
    public interface IAgentService : IService
    {
        event Action OnNumberOfAgentsChanged;

        int NumberOfAgents { get; }

        void SpawnAgent();
        void KillAgent();

        Vector3 GetRandomMapPosition();
    }
}