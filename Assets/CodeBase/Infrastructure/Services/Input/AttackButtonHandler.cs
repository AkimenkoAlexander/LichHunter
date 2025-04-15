using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class AttackButtonHandler : MonoBehaviour, UIButton
    {
        private bool _isPressed;


        public bool IsPressed
        {
            get => _isPressed;
            set => _isPressed = value;
        }
    

        public void ButtonDown()
        {
            _isPressed = true;
        }

        public void ButtonUp()
        {
            ResetButtonState();
        }

        public bool IsAttackButtonDown() => IsPressed;
        public void ResetButtonState() => IsPressed = false;


    }
}