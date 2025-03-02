
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent: MonoBehaviour
    {
        private const float MinVelocity = 0.1f;
        
        public NavMeshAgent Agent;
        public EnemyAnimator Animator;


        private void Update()
        {
            if (ShouldMove())
                Animator.Move(Agent.velocity.magnitude);
            else
                Animator.StopMoving();
                
        }

        private bool ShouldMove() => 
            Agent.velocity.magnitude > MinVelocity && Agent.remainingDistance > Agent.radius;
    }
}