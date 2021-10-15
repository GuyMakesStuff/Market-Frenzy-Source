using System.Collections;
using MarketFrenzy.Managers;
using UnityEngine;

namespace MarketFrenzy
{
    public class Trophy : MonoBehaviour
    {
        public GameObject NewText;
        public Transform TrophyPrefabSpawnPoint;
        public Transform ArrowPos;
        GameObject trophy;
        ProgressManager.Achievement Achievement;
        [HideInInspector]
        public bool IsUnlocked;

        public void Init(int AchievementIndex)
        {
            Achievement = ProgressManager.Instance.Achievements[AchievementIndex];
            trophy = Instantiate(Achievement.Prefab, TrophyPrefabSpawnPoint.position, Random.rotation, TrophyPrefabSpawnPoint);
        }

        void LateUpdate()
        {
            IsUnlocked = Achievement.Unlocked;
            trophy.SetActive(Achievement.Unlocked);
            NewText.SetActive(Achievement.IsNew);
        }
    }
}

