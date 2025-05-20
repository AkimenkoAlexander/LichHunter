using System;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;
        private IGameFactory gameFactory;
        private int minLoot;
        private int maxLoot;

        public void Construct(IGameFactory gameFactory, int minLoot,int maxLoot)
        {
            this.gameFactory = gameFactory;
            SetLoot(minLoot, maxLoot);
        }

        private void SetLoot(int min, int max)
        {
           minLoot = min;
           maxLoot = max;
        }

        private void Start()
        {
            _enemyDeath.HappenedDeath += SpawnLoot;
        }

        private void SpawnLoot()
        {
            GameObject loot = gameFactory.CreateLoot();
            loot.transform.position = transform.position;
        }
    }
}