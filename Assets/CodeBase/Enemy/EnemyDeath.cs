using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator), typeof(AgentMoveToPlayer))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private GameObject _deathVFX;
        [SerializeField] private AgentMoveToPlayer _moveToPlayer;

        public event Action Happened;

        private void Start() =>
            _health.ChangeHP += HealthChanged;

        private void OnDestroy() => 
            _health.ChangeHP -= HealthChanged;

        private void HealthChanged()
        {
            if (_health.CurrentHP <=0) Die();
        }

        private void Die()
        {
          _animator.PlayDeath();
          Instantiate(_deathVFX, transform.position, Quaternion.identity);
          _moveToPlayer.enabled = false;
          Happened?.Invoke();
          Destroy(gameObject, 3.0f);
        }
    }
}