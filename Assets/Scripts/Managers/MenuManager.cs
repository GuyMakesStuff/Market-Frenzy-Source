using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using MarketFrenzy.Audio;
using MarketFrenzy.IO;

namespace MarketFrenzy.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [Space]
        [Header("Title Text")]
        public Transform TitleText;
        public float SinRange;
        public float SinSpeed;

        [Header("Options")]
        public Slider VolSlider;
        AudioMixer MasterMixer;
        public TMP_Dropdown QualDropdown;
        public TMP_Dropdown ResDropdown;
        Resolution[] Resolutions;
        int PrevResIndex;
        public Toggle FSToggle;
        bool PrevFS;
        public Toggle PPToggle;
        bool InOptions;
        [System.Serializable]
        public class Settings : Saveable
        {
            public float Vol;
            public int QualLevel;
            public int ResIndex;
            public bool FS;
            public bool PP;
        }
        public Settings settings;

        [Header("Other")]
        public GameObject AchievementNewMark;

        void Awake()
        {
            Init(this);

            AudioPlayer.Instance.SetMusicTrack("MenuTheme");

            MasterMixer = AudioPlayer.Instance.OutputMixerGroup.audioMixer;

            Settings LoadedSettings = SaveSystem.LoadObject(settings.FileName, Application.persistentDataPath) as Settings;
            int curResIndex = 0;
            InitRes(ref curResIndex);
            if (LoadedSettings == null)
            {
                float Vol = 0f;
                MasterMixer.GetFloat("Volume", out Vol);
                settings.Vol = Vol;
                settings.QualLevel = QualitySettings.GetQualityLevel();
                settings.ResIndex = curResIndex;
                settings.FS = Screen.fullScreen;
                settings.PP = PPManager.Enabled;
            }
            else
            {
                settings = LoadedSettings;
            }
            SetSettings();
            ApplySettings();
        }
        void SetSettings()
        {
            VolSlider.value = settings.Vol;
            QualDropdown.value = settings.QualLevel;
            ResDropdown.value = settings.ResIndex;
            FSToggle.isOn = settings.FS;
            PPToggle.isOn = settings.PP;
        }
        void Update()
        {
            float Rot = Mathf.Sin(Time.time * SinSpeed) * SinRange;
            TitleText.rotation = Quaternion.Euler(0, 180, Rot);

            bool IsNewAchievement = false;
            foreach (ProgressManager.Achievement A in ProgressManager.Instance.Achievements)
            {
                if(A.IsNew)
                {
                    IsNewAchievement = true;
                    break;
                }
            }
            AchievementNewMark.SetActive(IsNewAchievement);

            if (InOptions)
            {
                ApplySettings();
                UpdateSettings();
            }
        }
        void UpdateRes()
        {
            Resolution Res = Resolutions[ResDropdown.value];
            Screen.SetResolution(Res.width, Res.height, FSToggle.isOn);
        }
        void InitRes(ref int CurResIndex)
        {
            Resolutions = Screen.resolutions;
            Resolution CurRes = Screen.currentResolution;
            List<string> Res2String = new List<string>();
            ResDropdown.ClearOptions();
            int curResIndex = 0;
            for (int R = 0; R < Resolutions.Length; R++)
            {
                Resolution Res = Resolutions[R];
                string String = Res.width + "x" + Res.height;
                Res2String.Add(String);

                if (Res.width == CurRes.width && Res.height == CurRes.height)
                {
                    curResIndex = R;
                }
            }
            CurResIndex = curResIndex;
            ResDropdown.AddOptions(Res2String);
        }
        void UpdateSettings()
        {
            settings.Vol = VolSlider.value;
            settings.QualLevel = QualDropdown.value;
            settings.ResIndex = ResDropdown.value;
            settings.FS = FSToggle.isOn;
            settings.PP = PPToggle.isOn;
            settings.Save();
        }
        void ApplySettings()
        {
            MasterMixer.SetFloat("Volume", VolSlider.value);
            QualitySettings.SetQualityLevel(QualDropdown.value);
            if (ResDropdown.value != PrevResIndex) { PrevResIndex = ResDropdown.value; UpdateRes(); }
            if (FSToggle.isOn != PrevFS) { PrevFS = FSToggle.isOn; UpdateRes(); }
            PPManager.Enabled = PPToggle.isOn;
        }

        public void SetInOptions(bool Value)
        {
            AudioPlayer.Instance.InteractWithSound("Beep", SoundBehaviourType.Play);
            InOptions = Value;
        }

        public void ClearSave()
        {
            ProgressManager.ShouldSave = false;
            ProgressManager.Instance.progress.Clear();
            QuitGame();
        }

        public void LoadScene(string sceneName)
        {
            FadeManager.Instance.FadeTo(sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}