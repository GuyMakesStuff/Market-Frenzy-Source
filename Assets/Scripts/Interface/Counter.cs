using System.Collections;
using TMPro;
using UnityEngine;
using MarketFrenzy.Audio;

namespace MarketFrenzy.Interface
{
    public class Counter : MonoBehaviour
    {
        public string ValueName;
        public int Value;
        public TMP_Text ValueText;
        public int HighValue;
        public TMP_Text HighValueText;
        public TMP_Text NewHIText;
        bool IsNewHI;
        Color CurNewHIColor;
        Color[] NewHICols = new Color[4]
        {
            Color.magenta,
            Color.yellow,
            Color.green,
            new Color(0, 1, 1, 1)
        };

        void Start()
        {
            StartCoroutine(ChangeColors());
        }

        void Update()
        {
            ValueText.text = ValueName + ":" + Value.ToString();
            HighValueText.text = "High " + ValueName + ":" + HighValue.ToString();

            NewHIText.enabled = IsNewHI;
            NewHIText.text = "New High " + ValueName + "!";
            NewHIText.color = CurNewHIColor;

            if (Value > HighValue)
            {
                HighValue = Value;
                if(IsNewHI == false)
                {
                    AudioPlayer.Instance.InteractWithSound("New HI", SoundBehaviourType.Play);
                }
                IsNewHI = true;
            }
        }

        IEnumerator ChangeColors()
        {
            for (int C = 0; C < NewHICols.Length; C++)
            {
                CurNewHIColor = NewHICols[C];
                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine(ChangeColors());
        }
    }
}