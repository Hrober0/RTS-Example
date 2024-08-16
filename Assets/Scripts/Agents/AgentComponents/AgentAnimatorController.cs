using UnityEngine;

namespace Agents
{
    public class AgentAnimatorController : MonoBehaviour
    {
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIDJump = Animator.StringToHash("Jump");
        private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        [SerializeField] private Animator _animator;
        [SerializeField] private Agent _agent;

        private void Start()
        {
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetFloat(_animIDMotionSpeed, 1.5f);
        }

        private void OnEnable()
        {
            _agent.OnMovementSpeedChanged += UpdateSpeed;
            UpdateSpeed();
        }

        private void OnDisable()
        {
            _agent.OnMovementSpeedChanged -= UpdateSpeed;
        }

        private void UpdateSpeed()
        {
            _animator.SetFloat(_animIDSpeed, _agent.MovementSpeed);

        }
    }
}