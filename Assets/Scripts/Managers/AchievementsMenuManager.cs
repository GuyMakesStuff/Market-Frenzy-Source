using System.Collections;
using TMPro;
using MarketFrenzy.Audio;
using MarketFrenzy.Interface;
using UnityEngine;

namespace MarketFrenzy.Managers
{
    public class AchievementsMenuManager : Manager<AchievementsMenuManager>
    {
        [Header("Trophy Placement")]
        public GameObject TrophyPrefab;
        public Transform StartTrophyPos;
        public float TrophyHoriGap;
        public int TrophyLineCount;
        public float TrophyVertGap;

        [Header("Navigation")]
        public PrevNextMenu AcheivmentsView;
        public Transform Arrow;
        public TMP_Text NameText;
        public TMP_Text DescriptionText;
        Trophy[] Trophies;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            AcheivmentsView.MinValue = 0;
            AcheivmentsView.MaxValue = ProgressManager.Instance.Achievements.Length - 1;
            SpawnTrophies();

            AudioPlayer.Instance.SetMusicTrack("SubMenuTheme");
        }

        // Update is called once per frame
        void Update()
        {
            Arrow.position = Trophies[AcheivmentsView.Value].ArrowPos.position;
            NameText.text = ProgressManager.Instance.Achievements[AcheivmentsView.Value].Name;
            DescriptionText.text = ProgressManager.Instance.Achievements[AcheivmentsView.Value].Description;
        }

        void SpawnTrophies()
        {
            int TrophiesSpawnedInLine = 0;
            Vector3 TrophySpawnPos = StartTrophyPos.position;
            Trophies = new Trophy[ProgressManager.Instance.Achievements.Length];
            for (int A = 0; A < ProgressManager.Instance.Achievements.Length; A++)
            {
                Trophy trophy = Instantiate(TrophyPrefab, TrophySpawnPos, TrophyPrefab.transform.rotation).GetComponent<Trophy>();
                trophy.Init(A);
                Trophies[A] = trophy;
                TrophySpawnPos.x += TrophyVertGap;
                TrophiesSpawnedInLine++;
                if(TrophiesSpawnedInLine == TrophyLineCount)
                {
                    TrophiesSpawnedInLine = 0;
                    TrophySpawnPos.x = StartTrophyPos.position.x;
                    TrophySpawnPos.y -= TrophyHoriGap;
                }
            }
        }

        public void Menu()
        {
            ProgressManager.Instance.UpdateAchievementNew();
            FadeManager.Instance.FadeTo("Menu");
        }
    }
}

