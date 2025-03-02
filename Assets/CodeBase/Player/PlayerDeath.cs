
using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeath: MonoBehaviour
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private PlayerMove _move;
        [SerializeField] private GameObject _deathVFX;

        private bool _isDeath;

        private void Start() =>
            _health.ChangeHP += HealthOnChangeHP;

        private void OnDestroy() =>
            _health.ChangeHP -= HealthOnChangeHP;


        private void HealthOnChangeHP()
        {
            if (!_isDeath && _health.CurrentHP <= 0)
            {
                _move.enabled = false;
                _animator.PlayDeath();
                Instantiate(_deathVFX, transform.position, Quaternion.identity);
            }
        }
    }
}