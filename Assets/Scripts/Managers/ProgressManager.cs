using MarketFrenzy.Interface;
using UnityEngine;
using MarketFrenzy.IO;
using MarketFrenzy.Audio;

namespace MarketFrenzy.Managers
{
    public class ProgressManager : Manager<ProgressManager>
    {
        [System.Serializable]
        public class Progress : Saveable
        {
            public int HIScore;
            public int HIRounds;
            public int Deaths;
            public int SelectedSkin;
            public bool[] SkinsUnlocked;
            public bool[] AchievementsUnlocked;
        }
        [System.Serializable]
        public class Achievement
        {
            public enum AchievementTypes { HIScore, HIRound, Deaths, AllItemsInShop}
            public AchievementTypes AchievementType;
            public string Name;
            [TextArea(3, 10)]
            public string Description;
            public GameObject Prefab;
            public bool Unlocked;
            public bool IsNew;
            public int Goal;

            public void ManualUpdate(Progress progress, int AchievementIndex, ProgressManager ProgMGR)
            {
                switch (AchievementType)
                {
                    case AchievementTypes.HIScore:
                    {
                        if(!Unlocked && progress.HIScore >= Goal)
                        {
                            Unlocked = true;
                            progress.AchievementsUnlocked[AchievementIndex] = true;
                            IsNew = true;
                            ProgMGR.SpawnAchievementMadeText(Name);
                            AudioPlayer.Instance.InteractWithSound("Achievement Made", SoundBehaviourType.Play);
                        }
                        break;
                    }
                    case AchievementTypes.HIRound:
                    {
                        if (!Unlocked && progress.HIRounds >= Goal)
                        {
                            Unlocked = true;
                            progress.AchievementsUnlocked[AchievementIndex] = true;
                            IsNew = true;
                            ProgMGR.SpawnAchievementMadeText(Name);
                            AudioPlayer.Instance.InteractWithSound("Achievement Made", SoundBehaviourType.Play);
                        }
                        break;
                    }
                    case AchievementTypes.Deaths:
                    {
                        if (!Unlocked && progress.Deaths >= Goal)
                        {
                            Unlocked = true;
                            progress.AchievementsUnlocked[AchievementIndex] = true;
                            IsNew = true;
                            ProgMGR.SpawnAchievementMadeText(Name);
                            AudioPlayer.Instance.InteractWithSound("Achievement Made", SoundBehaviourType.Play);
                        }
                        break;
                    }
                    case AchievementTypes.AllItemsInShop:
                    {
                        int UnlockCount = 0;
                        for (int S = 0; S < progress.SkinsUnlocked.Length; S++)
                        {
                            if(progress.SkinsUnlocked[S])
                            {
                                UnlockCount++;
                            }
                        }

                        if(!Unlocked && UnlockCount == progress.SkinsUnlocked.Length)
                        {
                            Unlocked = true;
                            progress.AchievementsUnlocked[AchievementIndex] = true;
                            IsNew = true;
                            ProgMGR.SpawnAchievementMadeText(Name);
                            AudioPlayer.Instance.InteractWithSound("Achievement Made", SoundBehaviourType.Play);
                        }
                        break;
                    }
                }
            }
        }
        public Progress progress;
        public Achievement[] Achievements;
        public GameObject PopUpTextPrefab;
        public static bool ShouldSave;

        void Start()
        {
            Init(this);

            ShouldSave = true;

            Progress LoadedProgress = SaveSystem.LoadObject(progress.FileName, Application.persistentDataPath) as Progress;
            if (LoadedProgress == null)
            {
                progress.HIScore = 0;
                progress.HIRounds = 0;
                progress.SelectedSkin = 0;
                progress.SkinsUnlocked = new bool[SkinManager.Instance.SkinCount];
                progress.SkinsUnlocked[0] = true;
                progress.AchievementsUnlocked = new bool[Achievements.Length];
            }
            else
            {
                progress = LoadedProgress;
                SkinManager.Instance.SelectedIndex = progress.SelectedSkin;
                SkinManager.Instance.SkinsUnlocked = progress.SkinsUnlocked;
            }

            for (int A = 0; A < Achievements.Length; A++)
            {
                Achievements[A].Unlocked = progress.AchievementsUnlocked[A];
            }

            SkinManager.Instance.AltStart();

            FadeManager.Instance.FadeTo("Menu");
        }

        void Update()
        {
            progress.SelectedSkin = SkinManager.Instance.SelectedIndex;
            progress.SkinsUnlocked = SkinManager.Instance.SkinsUnlocked;

            for (int A = 0; A < Achievements.Length; A++)
            {
                Achievements[A].ManualUpdate(progress, A, this);
            }

            if (ShouldSave)
            {
                progress.Save();
            }
        }

        public void UpdateAchievementNew()
        {
            for (int A = 0; A < Achievements.Length; A++)
            {
                Achievements[A].IsNew = false;
            }
        }

        public void SpawnPopUpText(Vector3 Pos, string text, Color Col, float Scale)
        {
            Canvas ParentCanvas = null;
            foreach (Canvas C in FindObjectsOfType<Canvas>())
            {
                if(C.gameObject.name != FadeManager.Instance.FadeCanvas.name)
                {
                    ParentCanvas = C;
                }
            }

            Debug.Log("Test");

            PopUpText popUp = Instantiate(PopUpTextPrefab, Pos, Quaternion.identity, ParentCanvas.transform).GetComponent<PopUpText>();
            popUp.Text = text;
            popUp.color = Col;
            popUp.Size = Scale;
        }
        protected void SpawnAchievementMadeText(string AchievementName)
        {
            Transform ScreenCenter = GameObject.FindGameObjectWithTag("Screen Center").transform;
            SpawnPopUpText(ScreenCenter.position, "Achievement Made!-\n" + AchievementName, Color.white, 108f);
        }
    }
}