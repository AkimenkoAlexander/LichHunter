using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        public event Action ChangeHP;
        
        float CurrentHP { get; set; }
        float MaxHP { get; set; }
        
        public void TakeDamage( float damage);
    }
}
