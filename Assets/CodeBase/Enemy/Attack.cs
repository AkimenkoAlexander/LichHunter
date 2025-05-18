using System.Linq;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Enemy
{
    public class Attack : MonoBehaviour
    {
        public float AttackCooldown;
        
        [SerializeField] private EnemyAnimator _animator;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10.0f;
        public  float Cleavage = 0.5f;
        
        private float _attackCooldown;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private Transform _playerTransform;
        private bool _isAttacking;
        private bool _attackIsActive;



        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Awake() => _layerMask = 1 << LayerMask.NameToLayer("Player");


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
                PhysicsDebug.DrawDebug(StartPoint(),Cleavage,1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
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
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hit;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f,transform.position.z) + transform.forward * EffectiveDistance;

        private bool CooldownIsUp() =>
            _attackCooldown <= 0f;



      
    }
}