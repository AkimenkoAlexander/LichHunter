using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _pickupFXPrefab;
        [SerializeField] private GameObject _skullPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            Debug.Log("Picked up loot");
            if (_picked) return;
            _picked = true;
            
            UpdateWorldData();
            HideSkull();
            PlayPickupFX();
            ShowText();
            StartCoroutine(StartDestoryTimer());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => _skullPrefab.SetActive(false);

        private IEnumerator StartDestoryTimer()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        private void PlayPickupFX() => 
            Instantiate(_pickupPopup, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = _loot.Value.ToString();
            _pickupPopup.SetActive(true);
        }
    }
}