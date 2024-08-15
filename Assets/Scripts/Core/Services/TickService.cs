using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TickService : MonoBehaviour, ITickService
    {
        public event Action OnGameSpeedChanged;

        public float GameSpeed { get; private set; }

        IEnumerator IService.Init()
        {
            yield return null;
        }

        void IService.Deinit()
        {

        }

        public void SetGameSpeed(float speed)
        {
            GameSpeed = speed;
            OnGameSpeedChanged?.Invoke();
        }
    }
}
