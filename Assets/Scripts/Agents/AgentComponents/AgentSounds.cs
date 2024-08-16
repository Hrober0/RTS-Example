using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agents
{
    public class AgentSounds : MonoBehaviour
    {
        [SerializeField] private AgentAnimatorEvents animatorEvents;
        [SerializeField] private AudioClip[] _footstepAudioClips;
        [SerializeField] private AudioClip _landingAudioClip;

        private void OnEnable()
        {
            animatorEvents.OnFootstepEvent += PlayFootstepSound;
            animatorEvents.OnLandEvent += PlayLandingSound;
        }

        private void OnDisable()
        {
            animatorEvents.OnFootstepEvent -= PlayFootstepSound;
            animatorEvents.OnLandEvent -= PlayLandingSound;
        }

        private void PlayFootstepSound()
        {
            var index = Random.Range(0, _footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.TransformPoint(transform.position), 0.5f);
        }

        private void PlayLandingSound()
        {
            AudioSource.PlayClipAtPoint(_landingAudioClip, transform.TransformPoint(transform.position), 0.5f);
        }
    }
}