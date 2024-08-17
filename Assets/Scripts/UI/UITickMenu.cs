using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITickMenu : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _gameSpeedLabel;
        [SerializeField] private TextMeshProUGUI _pauseButtonLabel;

        private ITickService _tickService;

        private void OnEnable()
        {
            _tickService = ServiceManager.Instance.Get<ITickService>();

            _tickService.OnGameSpeedChanged += UpdateGameSpeedLabel;
            UpdateGameSpeedLabel();

            _tickService.OnPauseStateChanged += UpdatePasueButtonLabel;
            UpdatePasueButtonLabel();

            _pauseButton.onClick.AddListener(ChangePauseState);
        }

        private void OnDisable()
        {
            _tickService.OnGameSpeedChanged -= UpdateGameSpeedLabel;
            
            _tickService.OnPauseStateChanged -= UpdatePasueButtonLabel;
            
            _pauseButton.onClick.RemoveListener(ChangePauseState);
        }

        public void ChangeGameSpeed(float value)
        {
            _tickService.SetGameSpeed(_tickService.GameSpeed + value);
        }

        private void UpdateGameSpeedLabel()
        {
            _gameSpeedLabel.text = $"{_tickService.GameSpeed:F2}";
        }

        private void ChangePauseState()
        {
            _tickService.SetPause(!_tickService.IsPaused);
        }

        private void UpdatePasueButtonLabel()
        {
            _pauseButtonLabel.text = _tickService.IsPaused ? "Unpause" : "Pause";
        }
    }
}
