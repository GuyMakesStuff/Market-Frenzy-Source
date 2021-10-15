using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using MarketFrenzy.Audio;

namespace MarketFrenzy.Managers
{
    public class FadeManager : Manager<FadeManager>
    {
        PPManager PP;
        public GameObject FadeCanvas;
        public Animator Fade;
        bool IsFading;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);
            PP = PPManager.Instance;
            FadeCanvas.transform.SetParent(transform);
        }
        void Update()
        {
            FadeCanvas.SetActive(!PPManager.Enabled);

            if (FadeCanvas.activeSelf)
            {
                Fade.SetBool("IsFaded", IsFading);
            }

            PP.TransitionInOut = !IsFading;
        }

        public void FadeTo(string SceneName)
        {
            StartCoroutine(fadeTo(SceneName));
        }
        IEnumerator fadeTo(string sceneName)
        {
            AudioPlayer.Instance.InteractWithSound("Fade In", SoundBehaviourType.Play);
            IsFading = true;
            SetUIEnabled(false);
            yield return new WaitForSeconds(1f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                yield return null;
            }
            IsFading = false;
            AudioPlayer.Instance.InteractWithSound("Fade Out", SoundBehaviourType.Play);
            SetUIEnabled(true);
        }

        void SetUIEnabled(bool Value)
        {
            foreach (Canvas C in FindObjectsOfType<Canvas>())
            {
                if (C.gameObject.name != FadeCanvas.gameObject.name)
                {
                    C.gameObject.SetActive(Value);
                }
            }
        }
    }
}