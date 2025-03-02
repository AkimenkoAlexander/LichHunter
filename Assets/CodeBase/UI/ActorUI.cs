
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HPBar _hpBar;

        private IHealth _healthStatus;

        private void OnDestroy ()
        {
            if(_healthStatus != null) _healthStatus.ChangeHP -= UpdateHPBar;
        }

        public void Construct(IHealth stateHealth)
        {
            _healthStatus = stateHealth;
            _healthStatus.ChangeHP += UpdateHPBar;

        }

        private void UpdateHPBar() => 
            _hpBar.SetValue(_healthStatus.CurrentHP, _healthStatus.MaxHP);
    }
}
