using MarketFrenzy.Audio;
using MarketFrenzy.Interface;
using UnityEngine;
using TMPro;

namespace MarketFrenzy.Managers
{
    public class TutorialManager : Manager<TutorialManager>
    {
        [System.Serializable]
        public class TutorialPage
        {
            public string Name;
            public GameObject OBJ;
            [TextArea(3, 10)]
            public string Description;
        }
        public TutorialPage[] Pages;
        public TMP_Text NameText;
        public TMP_Text DescriptionText;
        public PrevNextMenu SelectionMenu;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            AudioPlayer.Instance.SetMusicTrack("SubMenuTheme");

            SelectionMenu.MinValue = 0;
            SelectionMenu.MaxValue = Pages.Length - 1;
        }

        // Update is called once per frame
        void Update()
        {
            NameText.text = Pages[SelectionMenu.Value].Name;
            DescriptionText.text = Pages[SelectionMenu.Value].Description;

            for (int P = 0; P < Pages.Length; P++)
            {
                Pages[P].OBJ.SetActive((P == SelectionMenu.Value));
            }
        }

        public void Menu()
        {
            FadeManager.Instance.FadeTo("Menu");
        }
    }
}