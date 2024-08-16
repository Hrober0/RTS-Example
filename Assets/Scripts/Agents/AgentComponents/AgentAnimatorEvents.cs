using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agents
{
    public class AgentAnimatorEvents : MonoBehaviour
    {
        public event Action OnFootstepEvent;
        public event Action OnLandEvent;

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                OnFootstepEvent?.Invoke();
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                OnLandEvent?.Invoke();
            }
        }
    }
}