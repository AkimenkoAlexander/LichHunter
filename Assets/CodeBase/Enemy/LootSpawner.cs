using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Random = UnityEngine.Random;

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


        private void Start() => _enemyDeath.HappenedDeath += SpawnLoot;

        private void SpawnLoot()
        {
            LootPiece loot = gameFactory.CreateLoot();
            loot.transform.position = transform.position;
            Loot lootItem = GenerateLoot();
                
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot
            {
                Value = Random.Range(minLoot, maxLoot)
            };
        }

        private void SetLoot(int min, int max)
        {
           minLoot = min;
           maxLoot = max;
        }
    }
}