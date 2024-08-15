using System;

namespace Core
{
    public interface ITickService : IService
    {
        event Action OnGameSpeedChanged;
        event Action OnPauseStateChanged;

        float GameSpeed { get; }
        bool IsPaused { get; }

        void SetGameSpeed(float speed);
        void SetPause(bool pause);
    }
}
