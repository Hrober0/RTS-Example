using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface ITickService : IService
    {
        event Action OnGameSpeedChanged;

        float GameSpeed { get; }

        void SetGameSpeed(float speed);
    }
}
