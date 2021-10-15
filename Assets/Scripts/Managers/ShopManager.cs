using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MarketFrenzy.Audio;
using MarketFrenzy.Interface;

namespace MarketFrenzy.Managers
{
    public class ShopManager : Manager<ShopManager>
    {
        public PrevNextMenu SelectionMenu;
        public Material DummyPlayerMat;
        public TMP_Text PriceText;
        public TMP_Text HIText;
        public GameObject BuyButton;
        public GameObject SelectButton;


        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            AudioPlayer.Instance.SetMusicTrack("SubMenuTheme");

            SelectionMenu.Value = SkinManager.Instance.SelectedIndex;
            SelectionMenu.MinValue = 0;
            SelectionMenu.MaxValue = SkinManager.Instance.Skins.Length - 1;
        }

        // Update is called once per frame
        void Update()
        {
            DummyPlayerMat.color = SkinManager.Instance.Skins[SelectionMenu.Value].SkinColor;

            PriceText.text = "Price:" + SkinManager.Instance.Skins[SelectionMenu.Value].Price.ToString("00000");
            HIText.text = "High Score:" + ProgressManager.Instance.progress.HIScore.ToString("000");

            BuyButton.SetActive(!SkinManager.Instance.Skins[SelectionMenu.Value].Unlocked);
            SelectButton.SetActive(SkinManager.Instance.Skins[SelectionMenu.Value].Unlocked && ProgressManager.Instance.progress.SelectedSkin != SelectionMenu.Value);
        }

        public void Buy()
        {
            if (ProgressManager.Instance.progress.HIScore > SkinManager.Instance.Skins[SelectionMenu.Value].Price)
            {
                ProgressManager.Instance.progress.HIScore -= SkinManager.Instance.Skins[SelectionMenu.Value].Price;
                SkinManager.Instance.SkinsUnlocked[SelectionMenu.Value] = true;
                AudioPlayer.Instance.InteractWithSound("Buy", SoundBehaviourType.Play);
            }
            else
            {
                AudioPlayer.Instance.InteractWithSound("Cant Buy", SoundBehaviourType.Play);
            }
        }
        public void Select()
        {
            SkinManager.Instance.SelectedIndex = SelectionMenu.Value;
            AudioPlayer.Instance.InteractWithSound("Good Conveyor Remove", SoundBehaviourType.Play);
        }

        public void Menu()
        {
            FadeManager.Instance.FadeTo("Menu");
        }
    }
}