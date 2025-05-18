using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToPlayer : Follow
    {
        public NavMeshAgent Agent;
        private Transform _player;

        public void Construct(Transform herTransform) =>
            _player = herTransform;

        void Update() => 
            SetDestinationForPlayer();

        private void SetDestinationForPlayer()
        {
            if (_player != null)
            {
                Agent.SetDestination(_player.position);
            }
        }
    }
}