using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Follow _follow;
        [SerializeField] private float _cooldown;
        
        private bool hasAggroTarget;
        private Coroutine aggroCoroutine;
        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;
        }

        
        private void TriggerEnter( Collider obj)
        {
            if (!hasAggroTarget)
            {
                SwitchFollowOn();
                hasAggroTarget = true;
                StopAggroCoroutine();
            }
         
        }

        private void TriggerExit( Collider obj)
        {
            if (hasAggroTarget)
            {
                aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
                hasAggroTarget = false;
            }
        }

        private void StopAggroCoroutine()
        {
            if (aggroCoroutine != null)
            {
                StopCoroutine(aggroCoroutine);
                aggroCoroutine = null;  
            }
        }

        IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            SwitchFollowOff();
            hasAggroTarget = false;
        }

        private void SwitchFollowOn() => _follow.enabled = true;
        private void SwitchFollowOff() => _follow.enabled = false;
    }
}