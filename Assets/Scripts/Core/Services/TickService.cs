using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TickService : MonoBehaviour, ITickService
    {
        public event Action OnGameSpeedChanged;
        public event Action OnPauseStateChanged;

        public float GameSpeed { get; private set; }
        public bool IsPaused { get; private set; }

        IEnumerator IService.Init()
        {
            GameSpeed = 1.0f;
            yield return null;
        }

        void IService.Deinit()
        {

        }

        public void SetGameSpeed(float speed)
        {
            GameSpeed = Mathf.Max(speed, 0.1f);
            OnGameSpeedChanged?.Invoke();
        }
        
        public void SetPause(bool pause)
        {
            IsPaused = pause;
            OnPauseStateChanged?.Invoke();
        }
    }
}
