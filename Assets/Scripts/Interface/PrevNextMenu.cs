using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using MarketFrenzy.Audio;

namespace MarketFrenzy.Interface
{
    public class PrevNextMenu : MonoBehaviour
    {
        public Button PrevButton;
        public Button NextButton;
        public int MinValue;
        public int MaxValue;
        [HideInInspector]
        public int Value;

        // Start is called before the first frame update
        void Start()
        {
            PrevButton.onClick.AddListener(new UnityAction(Prev));
            NextButton.onClick.AddListener(new UnityAction(Next));
        }

        // Update is called once per frame
        void Update()
        {
            Value = Mathf.Clamp(Value, MinValue, MaxValue);
            PrevButton.gameObject.SetActive((Value != MinValue));
            NextButton.gameObject.SetActive((Value != MaxValue));
        }

        public void Prev()
        {
            AudioPlayer.Instance.InteractWithSound("Beep", SoundBehaviourType.Play);
            Value--;
        }
        public void Next()
        {
            AudioPlayer.Instance.InteractWithSound("Beep", SoundBehaviourType.Play);
            Value++;
        }
    }
}