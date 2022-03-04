using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("PlayerSettings")]
        public int maxLife;
        public float movementSpeed;
        public float invincibilityCooldown;

        [Header("CommonSettings")] 
        public int maxEnemies;
        public int maxCrystals;
        public int pointsForCrystal;
        [Range(1, 10)]
        public float spawnCrystalInterval;
        public float spawnEnemyInterval;

    }
}