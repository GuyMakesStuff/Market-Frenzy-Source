using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Range(0, 1)]
        public float Volume = 1f;
        public bool Looping;
        [HideInInspector]
        public AudioSource OutputSource;
        public bool IgnoresAllInteraction;
    }
}