using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Attack : MonoBehaviour
    {
        public float AttackCooldown;
        
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private float _damage = 10.0f;

        private float _attackCooldown;
        private IGameFactory _gameFactory;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private Transform _playerTransform;
        private bool _isAttacking;
        private bool _attackIsActive;
        private float _cleavage = 0.5f;
 

        private void Awake()
        {
            _gameFactory = AllServices.Container().Single<IGameFactory>();
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Start() => 
            InitializePlayerCreatedHandler();

        private void Update()
        {
            UpdateCoolDown();
            CanAttack();
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() =>
            _attackIsActive = false;

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(),_cleavage,1);
                hit.transform.GetComponent<IHealth>().TakeDamage(_damage);
            }
        }

        private void StartAttack()
        {
            transform.LookAt(_playerTransform);
            _animator.PlayAttack();
            _isAttacking = true;
        }

        private void CanAttack()
        {
            if (_attackIsActive && !_isAttacking && CooldownIsUp()) 
                StartAttack();
        }

        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
            
        }

        private void UpdateCoolDown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hit;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f,transform.position.z) + transform.forward * _effectiveDistance;

        private bool CooldownIsUp() =>
            _attackCooldown <= 0f;

        private void OnPlayerCreated() => 
            _playerTransform = _gameFactory.PlayerGameObject.transform;

        private void InitializePlayerCreatedHandler()
        {
            if (_gameFactory.PlayerGameObject != null)
            {
                OnPlayerCreated();
            }
            else
            {
                _gameFactory.PlayerCreated += OnPlayerCreated;
            }
        }
    }
}