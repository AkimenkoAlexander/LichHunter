using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterStaticData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;
        [Range(1,100)]
        public int Hp;
        [Range(1,30)]
        public float Damage;
        [Range(1, 30)]
        public int LootMin;
        [Range(31, 50)]
        public int LootMax;
        [Range(1,50)]
        public float MoveSpeed;
        [Range(0.5f,1)]
        public float EffectiveDistance;
        [Range(0.5f,1)]
        public float Cleavage;
        
        public GameObject Prefab;
    }
    
}