using System.Collections;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(CharacterController))]
    public class PlayerAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private CharacterController _controller;
        
        private IGameFactory _gameFactory;
        private Stats _stats;
        
        private AttackButtonHandler _attackButton;
        
        private bool _timeFirstAttack = true;
        private bool _timeNextAttack;
        
        
        private Coroutine _attackCooldown;
        private Collider[] _hits = new Collider[7];
        
        private  static int _layerMask;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
            _gameFactory = AllServices.Container().Single<IGameFactory>(); 
            _gameFactory.HudCreated += InstAttackButton;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _animator.PlayJumpAttack();
            }
            CheckInputAttackButton();
        }

        private void CheckInputAttackButton()
        {
            if (( _attackButton.IsAttackButtonDown() || Input.GetKeyDown(KeyCode.F)) && !_animator.IsAttacking )
            {
                if (_timeNextAttack)
                {
                    _animator.PlayNextAttack();
                    if (_attackCooldown != null)
                    {
                        StopCoroutine(TimeForNextAttack());
                        _attackCooldown = null;
                    }
                    _timeNextAttack = false;
                }
                else if(!_timeNextAttack && _attackCooldown == null && _timeFirstAttack)
                {
                    _timeNextAttack = true;
                    _animator.PlayAttack();
                    _attackCooldown =  StartCoroutine(TimeForNextAttack());
                    _timeFirstAttack = false;
                }
                
                _attackButton.ResetButtonState();
            }
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
               _hits[i].transform.parent.GetComponentInParent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        private int Hit()
        {
            PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius,2);
            return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits,_layerMask);
        }
        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, transform.position.y + _controller.center.y / 2 , transform.position.z);
        private void InstAttackButton() =>
            _attackButton = GetButtonAttack();
        private AttackButtonHandler GetButtonAttack() => 
            _gameFactory.Hud.GetComponentInChildren<AttackButtonHandler>();

        private void FinishLastAttack()
        {
            _animator.FinishingNextAttack();
        }
        public void LoadProgress(PlayerProgress progress) => _stats = progress.PlayerStats;

        private IEnumerator TimeForNextAttack()
        {
            yield return new WaitForSeconds(1);
            _timeNextAttack = false;
            _attackCooldown = null;
            _timeFirstAttack = true;
        }
        private void OnDestroy() => 
            _gameFactory.HudCreated -= InstAttackButton;
    }
}