using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _asset;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public  GameFactory ( IAssetProvider asset)
        {
            _asset = asset;
        }

        public GameObject PlayerGameObject { get; set; }
        public GameObject Hud { get; set; }


        public event Action PlayerCreated;
        
        public event Action HudCreated;
       


        public GameObject CreatePlayer(GameObject at)
        {
            PlayerGameObject = InstantiateRegister(AssetPath.PlayerPrefabPath, at.transform);
            PlayerCreated?.Invoke();
            return PlayerGameObject;
        }

        public GameObject CreateHud()
        {
            Hud =  InstantiateRegister(AssetPath.HudPath);
            HudCreated?.Invoke();
            return Hud;
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