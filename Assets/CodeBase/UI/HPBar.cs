using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
   public class HPBar : MonoBehaviour
   {
      [SerializeField] private Image _currentHPImage;
      public void SetValue(float currentHP,float maxHP) => 
          _currentHPImage.fillAmount = currentHP / maxHP;
   }
}
