using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _current;
        [SerializeField] private float _max;
     
        
        public event Action ChangeHP;

        public float CurrentHP
        {
            get => _current;
            set => _current = value;
        }

        public float MaxHP
        {
            get => _max;
            set => _max = value;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("TakeDamage");
            _current -= damage;
            _animator.PlayHit();
            ChangeHP?.Invoke();
        }
    }
}