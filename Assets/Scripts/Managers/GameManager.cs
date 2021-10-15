using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MarketFrenzy.Interface;
using MarketFrenzy.Audio;
using MarketFrenzy.Gameplay;
using MarketFrenzy.Gameplay.ConveyorItems;
using MarketFrenzy.Gameplay.Obstacles;

namespace MarketFrenzy.Managers
{
    public class GameManager : Manager<GameManager>
    {
        Player player;
        public static bool IsPaused;
        public static bool AllowedToPause;

        [Header("UI")]
        public Counter ScoreCounter;
        public Counter RoundCounter;
        public Transform ScorePopUpSpawnPoint;
        [HideInInspector]
        public int Lives;
        public GameObject[] Hearts;
        public TMP_Text NextRoundScoreAmountText;
        public Color[] TimerColors;
        public TMP_Text TimerText;
        public Image RedFade;

        [Header("Rounds")]
        public int StartNextRoundScoreAmount;
        public int NextRoundScoreAmountAddon;
        int NextRoundScoreAmount;
        public Animator NextRoundText;

        [Header("Mini Menus")]
        public GameObject PauseMenu;
        public GameObject GameOverMenu;
        public TMP_Text DeathMessageText;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            player = FindObjectOfType<Player>();
            Lives = Hearts.Length;
            RoundCounter.Value = 1;
            ScoreCounter.HighValue = ProgressManager.Instance.progress.HIScore;
            RoundCounter.HighValue = ProgressManager.Instance.progress.HIRounds;
            NextRoundScoreAmount = StartNextRoundScoreAmount;
            AudioPlayer.Instance.MuteMusic();
            StartCoroutine(PlayTimerOnStart());
        }
        IEnumerator PlayTimerOnStart()
        {
            IsPaused = true;
            AllowedToPause = false;
            AudioPlayer.Instance.InteractWithSound("Drums", SoundBehaviourType.Play);
            yield return new WaitForSeconds(1f);
            StartCoroutine(StartTimer());
        }

        // Update is called once per frame
        void Update()
        {
            if (ScoreCounter.Value >= NextRoundScoreAmount)
            {
                RoundCounter.Value++;
                NextRoundScoreAmount += StartNextRoundScoreAmount + (NextRoundScoreAmountAddon * (RoundCounter.Value - 1));
                SpawnManager.Instance.DecreaseSpawnDelay();
                Conveyor.Instance.EncreaseValues();
                AudioPlayer.Instance.MuteMusic();
                AudioPlayer.Instance.InteractWithSound("Next Round", SoundBehaviourType.Play);
                StartCoroutine(ShowNextRoundAnim());
            }

            NextRoundScoreAmountText.text = "Next Round Will Be When Score Reaches:" + NextRoundScoreAmount;

            ProgressManager.Instance.progress.HIScore = ScoreCounter.HighValue;
            ProgressManager.Instance.progress.HIRounds = RoundCounter.HighValue;

            for (int H = 0; H < Hearts.Length; H++)
            {
                Hearts[H].SetActive(((H + 1) <= Lives));
            }

            RedFade.enabled = !PPManager.Enabled;
            RedFade.color = Color.Lerp(new Color(1, 0, 0, 0), new Color(1, 0, 0, 0.75f), PPManager.Instance.RValue / 100f);

            if (Input.GetKeyDown(KeyCode.Escape) && AllowedToPause)
            {
                if (!IsPaused)
                {
                    AudioPlayer.Instance.MuteMusic();
                    AudioPlayer.Instance.InteractWithSound("Beep", SoundBehaviourType.Play);
                }
                IsPaused = true;
            }
            PauseMenu.SetActive((IsPaused && AllowedToPause));
            AudioPlayer.Instance.InteractWithAllSoundsOneShot((PauseMenu.activeSelf) ? SoundBehaviourType.Pause : SoundBehaviourType.Resume);
        }
        IEnumerator ShowNextRoundAnim()
        {
            IsPaused = true;
            AllowedToPause = false;
            player.gameObject.SetActive(false);
            foreach (ConveyorItem CI in FindObjectsOfType<ConveyorItem>())
            {
                Destroy(CI.gameObject);
            }
            foreach (Obstacle O in FindObjectsOfType<Obstacle>())
            {
                Destroy(O.gameObject);
            }
            NextRoundText.SetBool("IsShowen", true);
            yield return new WaitForSeconds(8.5f);
            NextRoundText.SetBool("IsShowen", false);
            yield return new WaitForSeconds(1f);
            player.gameObject.SetActive(true);
            StartCoroutine(StartTimer());
        }
        public IEnumerator StartTimer()
        {
            IsPaused = true;
            AllowedToPause = false;
            int TimerValue = 3;
            TimerText.gameObject.SetActive(true);
            AudioPlayer.Instance.InteractWithSound("Timer", SoundBehaviourType.Play);
            AudioPlayer.Instance.InteractWithSoundOneShot("Drums", SoundBehaviourType.Play);
            while (TimerValue > -1)
            {
                if (TimerValue <= 0)
                {
                    TimerText.text = "Go!";
                    AudioPlayer.Instance.InteractWithSound("Drums", SoundBehaviourType.Stop);
                    AudioPlayer.Instance.SetMusicTrack("MainTheme");
                    IsPaused = false;
                }
                else
                {
                    TimerText.text = TimerValue.ToString("0");
                }
                TimerText.color = TimerColors[TimerValue];

                yield return new WaitForSeconds(1f);
                TimerValue--;
            }

            AllowedToPause = true;
            TimerText.gameObject.SetActive(false);
        }

        public void Damage(string Message)
        {
            if (GameOverMenu.activeSelf)
            {
                return;
            }

            Lives--;
            if (Lives > 0)
            {
                AudioPlayer.Instance.InteractWithSound("Player Hit", SoundBehaviourType.Play);
                PPManager.Instance.ShowDamageFX();
            }
            else
            {
                Kill(Message);
            }
        }

        public void AddScore(int ValueToAdd)
        {
            if (GameOverMenu.activeSelf)
            {
                return;
            }

            if (ValueToAdd < 0)
            {
                Debug.LogError("Value Must Be Grater Than 0!");
                return;
            }

            ProgressManager.Instance.SpawnPopUpText(ScorePopUpSpawnPoint.position, "+" + ValueToAdd, Color.green, 40f);
            ScoreCounter.Value += ValueToAdd;
        }
        public void DecreaseScore(int DecreaseAmount)
        {
            if (GameOverMenu.activeSelf)
            {
                return;
            }

            if (DecreaseAmount < 0)
            {
                Debug.LogError("Value Must Be Grater Than 0!");
                return;
            }

            ProgressManager.Instance.SpawnPopUpText(ScorePopUpSpawnPoint.position, "-" + DecreaseAmount, Color.red, 40f);
            ScoreCounter.Value -= DecreaseAmount;

            if(ScoreCounter.Value < 0)
            {
                Kill("You Ran Out Of Score!");
            }
        }
        void Kill(string Message)
        {
            AudioPlayer.Instance.MuteMusic();
            AudioPlayer.Instance.InteractWithSound("Player Die", SoundBehaviourType.Play);
            PPManager.Instance.IsDying = true;
            ProgressManager.Instance.progress.Deaths++;
            player.ShowKillFX();
            AllowedToPause = false;
            DeathMessageText.text = Message;
            GameOverMenu.SetActive(true);
        }

        public void Resume(bool Lock)
        {
            PlayBeep();
            IsPaused = false;
            AllowedToPause = !Lock;
            if (!Lock)
            {
                AudioPlayer.Instance.SetMusicTrack("MainTheme");
            }
            PPManager.Instance.IsDying = false;
        }
        public void Retry()
        {
            Resume(true);
            FadeManager.Instance.FadeTo(SceneManager.GetActiveScene().name);
        }
        public void Menu()
        {
            Resume(true);
            FadeManager.Instance.FadeTo("Menu");
        }

        public void PlayBeep()
        {
            AudioPlayer.Instance.InteractWithSound("Beep", SoundBehaviourType.Play);
        }
    }
}