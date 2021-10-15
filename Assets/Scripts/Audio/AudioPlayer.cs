using System;
using UnityEngine.Audio;
using UnityEngine;
using MarketFrenzy.Managers;

namespace MarketFrenzy.Audio
{
    public class AudioPlayer : Manager<AudioPlayer>
    {
        public Sound[] SoundEffects;
        public AudioClip[] MusicTracks;
        [Range(0, 1)]
        public float MusicVolume;
        AudioSource MusicOutput;
        public AudioMixerGroup OutputMixerGroup;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);

            foreach (Sound S in SoundEffects)
            {
                S.OutputSource = new GameObject(S.Name).AddComponent<AudioSource>();
                S.OutputSource.transform.SetParent(transform);
                S.OutputSource.clip = S.Clip;
                S.OutputSource.volume = S.Volume;
                S.OutputSource.loop = S.Looping;
                S.OutputSource.outputAudioMixerGroup = OutputMixerGroup;
            }

            MusicOutput = new GameObject("Music").AddComponent<AudioSource>();
            MusicOutput.transform.SetParent(transform);
            MusicOutput.volume = MusicVolume;
            MusicOutput.loop = true;
            MusicOutput.outputAudioMixerGroup = OutputMixerGroup;
            MuteMusic();
        }

        public void InteractWithSound(string SoundName, SoundBehaviourType behaviourType)
        {
            Sound sound = Array.Find(SoundEffects, Sound => Sound.Name == SoundName);

            if(sound == null)
            {
                Debug.LogError("Sound-" + SoundName + " Does Not Exist On Audio Manager!");
                return;
            }

            switch (behaviourType)
            {
                case SoundBehaviourType.Play:
                {
                    sound.OutputSource.Play();
                    break;
                }
                case SoundBehaviourType.Stop:
                {
                    sound.OutputSource.Stop();
                    break;
                }
                case SoundBehaviourType.Pause:
                {
                    sound.OutputSource.Pause();
                    break;
                }
                case SoundBehaviourType.Resume:
                {
                    sound.OutputSource.UnPause();
                    break;
                }
            }
        }
        public void InteractWithSoundOneShot(string SoundName, SoundBehaviourType behaviourType)
        {
            Sound sound = Array.Find(SoundEffects, Sound => Sound.Name == SoundName);

            if(sound == null)
            {
                Debug.LogError("Sound-" + SoundName + " Does Not Exist On Audio Manager!");
                return;
            }

            switch (behaviourType)
            {
                case SoundBehaviourType.Play:
                {
                    if(!sound.OutputSource.isPlaying)
                    {
                        sound.OutputSource.Play();
                    }
                    break;
                }
                case SoundBehaviourType.Stop:
                {
                    if(sound.OutputSource.isPlaying)
                    {
                        sound.OutputSource.Stop();
                    }
                    break;
                }
                case SoundBehaviourType.Pause:
                {
                    sound.OutputSource.Pause();
                    break;
                }
                case SoundBehaviourType.Resume:
                {
                    sound.OutputSource.UnPause();
                    break;
                }
            }
        }

        public void InteractWithAllSounds(SoundBehaviourType behaviourType)
        {
            foreach (Sound S in SoundEffects)
            {
                if(!S.IgnoresAllInteraction)
                {
                    InteractWithSound(S.Name, behaviourType);
                }
            }
        }
        public void InteractWithAllSoundsOneShot(SoundBehaviourType behaviourType)
        {
            foreach (Sound S in SoundEffects)
            {
                if(!S.IgnoresAllInteraction)
                {
                    InteractWithSound(S.Name, behaviourType);
                }
            }
        }

        public void MuteMusic()
        {
            MusicOutput.clip = null;
            MusicOutput.Stop();
        }
        public void SetMusicTrack(string Name)
        {
            MuteMusic();
            AudioClip musicClip = Array.Find(MusicTracks, AudioClip => AudioClip.name == Name);
            if(musicClip == null)
            {
                Debug.LogError("The Music Track " + Name + " Does Not Exist In The Audio Manager!");
                return;
            }
            else if(musicClip == MusicOutput.clip)
            {
                Debug.LogWarning("The Music Track " + Name + " Is Already Playing!");
                return;
            }
            MusicOutput.clip = musicClip;
            MusicOutput.Play();
        }
    }
}
