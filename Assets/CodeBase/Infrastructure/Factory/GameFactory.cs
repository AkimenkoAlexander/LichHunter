using System;
using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _asset;
        private readonly IStaticDataService _statcData;
        private readonly IPersistentProgressService _progressService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameFactory(IAssetProvider asset, IStaticDataService statcData, IPersistentProgressService progressService)
        {
            _asset = asset;
            _statcData = statcData;
            _progressService = progressService;
        }

        public GameObject PlayerGameObject { get; set; }
        public GameObject Hud { get; set; }




        public event Action HudCreated;


        public GameObject CreatePlayer(GameObject at)
        {
            PlayerGameObject = InstantiateRegister(AssetPath.PlayerPrefabPath, at.transform);
            return PlayerGameObject;
        }

        public GameObject CreateHud()
        {
            Hud = InstantiateRegister(AssetPath.HudPath);
            HudCreated?.Invoke();
            return Hud;
        }

        public GameObject CreateMonsters(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _statcData.ForMonsters(typeId);
            GameObject monster =
                Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            monster.name = monsterData.name;
            var health = monster.GetComponent<IHealth>();
            health.CurrentHP = monsterData.Hp;
            health.MaxHP = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToPlayer>().Construct(PlayerGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
            monster.GetComponent<LootSpawner>().Construct(this, monsterData.LootMin, monsterData.LootMax);
            var attack = monster.GetComponent<Attack>();
            attack.Construct(PlayerGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Damage;
            attack.EffectiveDistance = monsterData.Damage;
            

            return monster;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegister(AssetPath.LootPrefabPath).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData);
                return lootPiece;
        }

        private GameObject InstantiateRegister(string prefabPath)
        {
            GameObject gameObject = _asset.Instantiate(prefabPath);
            return gameObject;
        }

        private GameObject InstantiateRegister(string prefabPath, Transform position)
        {
            GameObject gameObject = _asset.Instantiate(prefabPath, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }

            ProgressReaders.Add(progressReader);
        }
    }
}