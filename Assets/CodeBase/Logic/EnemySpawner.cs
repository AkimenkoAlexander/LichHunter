using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        private string _id;
        public bool Slain;
        
        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearSpawners.Contains(_id))
            {
                Slain = true;
            }
            else
                Spawn();
        }

        private void Spawn()
        {
         
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (Slain)
            {
                progress.KillData.ClearSpawners.Add(_id);
            }
        }
    }

}
