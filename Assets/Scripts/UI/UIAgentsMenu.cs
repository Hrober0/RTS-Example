using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace UI
{
    public class UIAgentsMenu : MonoBehaviour
    {
        [SerializeField] private Button _spawnAgentButton;
        [SerializeField] private Button _killAgentButton;
        [SerializeField] private Button _killAllAgentsButton;
        [SerializeField] private TextMeshProUGUI _agentsAmountLabel;

        private IAgentService _agentService;

        private void OnEnable()
        {
            _agentService = ServiceManager.Instance.Get<IAgentService>();
            
            _agentService.OnNumberOfAgentsChanged += UpdateAgentsAmount;
            UpdateAgentsAmount();

            _spawnAgentButton.onClick.AddListener(_agentService.SpawnAgent);
            _killAgentButton.onClick.AddListener(_agentService.KillAgent);
            _killAllAgentsButton.onClick.AddListener(KillAllAgents);
        }

        private void OnDisable()
        {
            _agentService.OnNumberOfAgentsChanged -= UpdateAgentsAmount;

            _spawnAgentButton.onClick.RemoveListener(_agentService.SpawnAgent);
            _killAgentButton.onClick.RemoveListener(_agentService.KillAgent);
            _killAllAgentsButton.onClick.RemoveListener(KillAllAgents);
        }

        private void UpdateAgentsAmount()
        {
            UpdateAgentsAmountLabel();
            UpdateButtonsInstaractable();
        }

        private void UpdateAgentsAmountLabel()
        {
            _agentsAmountLabel.text = $"{_agentService.NumberOfAgents}";
        }

        private void UpdateButtonsInstaractable()
        {
            _killAgentButton.interactable = _agentService.NumberOfAgents > 0;
            _killAllAgentsButton.interactable = _agentService.NumberOfAgents > 0;
        }

        private void KillAllAgents()
        {
            for (int i = 0; i < _agentService.NumberOfAgents; i++)
            {
                _agentService.KillAgent();
            }
        }
    }
}