using Core;

namespace Agents
{
    public struct AgentReachedTargetNotification : INotification
    {
        private Agent _agent;

        public AgentReachedTargetNotification(Agent agent)
        {
            _agent = agent;
        }

        public readonly string GetMessage() => $"Agent {_agent.Guid} arrived";
    }
}