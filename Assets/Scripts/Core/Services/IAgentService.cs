using System;
using UnityEngine;

namespace Core
{
    public interface IAgentService : IService
    {
        event Action OnNumberOfAgentsChanged;
        event Action<INotification> OnNotification;

        int NumberOfAgents { get; }

        void RequestAgentSpawn();
        void RequestAgentKill();

        Vector3 GetRandomMapPosition();
    }
}