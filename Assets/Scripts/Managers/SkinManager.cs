using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Managers
{
    public class SkinManager : Manager<SkinManager>
    {
        [System.Serializable]
        public class Skin
        {
            public Color SkinColor;
            public int Price;
            [HideInInspector]
            public bool Unlocked;
        }
        public Skin[] Skins;
        public Material PlayerMat;
        public int SelectedIndex;
        public bool[] SkinsUnlocked;
        public int SkinCount;
        int PrevSelectedIndex = -1;

        void Awake()
        {
            Init(this);
            SkinCount = Skins.Length;
        }

        // Start is called before the first frame update
        public void AltStart()
        {
            SkinsUnlocked = ProgressManager.Instance.progress.SkinsUnlocked;
        }

        // Update is called once per frame
        void Update()
        {
            if (SelectedIndex != PrevSelectedIndex)
            {
                PrevSelectedIndex = SelectedIndex;
                PlayerMat.color = Skins[SelectedIndex].SkinColor;
            }

            for (int S = 0; S < SkinCount; S++)
            {
                Skins[S].Unlocked = SkinsUnlocked[S];
            }
        }
    }
}