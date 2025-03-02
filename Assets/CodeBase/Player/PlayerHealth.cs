using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Player
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class PlayerHealth: MonoBehaviour, IHealth,ISavedProgress
    {
        public PlayerAnimator PlayerAnimator;
        private StateHealth _playerHealthState;
        public event Action ChangeHP;

        public float CurrentHP
        {
            
            get => _playerHealthState.CurrentHP;
            set
            {
                if (_playerHealthState.CurrentHP != value)
                {
                    _playerHealthState.CurrentHP = value;
                    ChangeHP?.Invoke();
                }
            }
        }

        public float MaxHP
        {
            get => _playerHealthState.MaxHP;
            set
            {
                _playerHealthState.MaxHP = value;
                ChangeHP?.Invoke();
            }
        }


        public void LoadProgress(PlayerProgress progress)
        {
           _playerHealthState = progress.StateHealth;
            ChangeHP?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.StateHealth.CurrentHP = CurrentHP;
            progress.StateHealth.MaxHP = MaxHP;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHP <=0)
            {
                return;
            }
            CurrentHP -= damage;
            PlayerAnimator.PlayHit();
        }
    }
}