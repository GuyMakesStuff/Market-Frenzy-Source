using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

namespace MarketFrenzy.Managers
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PPManager : Manager<PPManager>
    {
        PostProcessVolume Volume;
        public PostProcessProfile Profile;
        ColorGrading Col;
        LensDistortion Lens;
        public static bool Enabled;
        [HideInInspector]
        public bool IsDying;
        [HideInInspector]
        public float RValue;
        float LensIntensity;
        float LensScale;
        public bool TransitionInOut;

        // Awake Is Called in The Very Beginning Of The Game. Even Before Start!
        void Awake()
        {
            Init(this);
            Enabled = true;
            Volume = GetComponent<PostProcessVolume>();
            Volume.profile = Profile;
            Col = Profile.GetSetting<ColorGrading>();
            Lens = Profile.GetSetting<LensDistortion>();
            LensIntensity = -100;
            LensScale = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            Volume.enabled = Enabled;

            RValue = Mathf.Clamp(RValue, 2f, 100f);
            if (IsDying)
            {
                RValue += 25f * Time.deltaTime;
            }
            else
            {
                if (RValue > 2)
                {
                    RValue -= 98f * Time.deltaTime;
                }
            }
            Col.temperature.value = RValue;
            Col.tint.value = RValue;

            LensIntensity = Mathf.Clamp(LensIntensity, -100f, 0f);
            LensScale = Mathf.Clamp01(LensScale);
            if (TransitionInOut)
            {
                if (LensIntensity < 0f)
                {
                    LensIntensity += 100f * Time.deltaTime;
                }
                if (LensScale < 1f)
                {
                    LensScale += Time.deltaTime;
                }
            }
            else
            {
                if (LensIntensity > -100f)
                {
                    LensIntensity -= 100f * Time.deltaTime;
                }
                if (LensScale > 0.01f)
                {
                    LensScale -= Time.deltaTime;
                }
            }
            Lens.intensity.value = LensIntensity;
            Lens.scale.value = LensScale;
        }

        public void ShowDamageFX()
        {
            RValue = 100f;
        }
    }
}