using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        public bool Slain => _slain;


        private string _id;
        private IGameFactory _gameFactory;
        private bool _slain;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _gameFactory = AllServices.Container().Single<IGameFactory>();
        }


        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearSpawners.Contains(_id))
            {
                _slain = true;
            }
            else
                Spawn();
        }


        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain) progress.KillData.ClearSpawners.Add(_id);
        }

        private void Spawn()
        {
            GameObject monsters = _gameFactory.CreateMonsters(MonsterTypeId, transform);
            _enemyDeath = monsters.GetComponent<EnemyDeath>();
            if (_enemyDeath != null) _enemyDeath.HappenedDeath += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null) _enemyDeath.HappenedDeath -= Slay;
            _slain = true;
        }
    }
}