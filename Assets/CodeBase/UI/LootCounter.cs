
using CodeBase.Data;
using TMPro;
using UnityEngine;


namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;
        private WorldData worldData;
        
        public void Construct(WorldData worldData)
        {
            this.worldData = worldData;
            worldData.LootData.Changed += UpdateCounter;
        }
        private void Start() =>
            UpdateCounter();

        

        private void UpdateCounter()
        {
            _counter.text = $"{worldData.LootData.Collected}";
        }
    
    }
}