using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomScripts.Managers.Sound
{
    public class MusicManager : MonoBehaviourSingleton<MusicManager>
    {
        private AudioSource activeAudio;

        public List<MusicGroup> MusicGroups;

        private int currentGroup = 0;
        private int currentTrack = 0;

        private Coroutine musicEndCoroutine;

        public override void Awake()
        {
            base.Awake();

            GameSettings.OnSettingsChanged -= ToggleMusic;
            GameSettings.OnSettingsChanged += ToggleMusic;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameSettings.Instance.ToggleBackgroundMusic();
            }
        }

        private void ToggleMusic()
        {
            if (GameSettings.BackgroundMusic)
            {
                PlayNextTrack();
            }
            else
            {
                StopMusic();
            }
        }

        public void PlayNextTrack()
        {
            StopMusic();

            if (!GameSettings.BackgroundMusic)
                return;

            MusicGroup musicGroup = MusicGroups[currentGroup];

            activeAudio = musicGroup.MusicTracks[currentTrack % musicGroup.MusicTracks.Count];
            float musicLength = activeAudio.clip.length;
            activeAudio.Play();
            musicEndCoroutine = StartCoroutine(OnMusicEnd(musicLength));
        }

        public void StopMusic()
        {
            if (activeAudio)
                activeAudio.Stop();

            if (musicEndCoroutine != null)
                StopCoroutine(musicEndCoroutine);
        }

        private IEnumerator OnMusicEnd(float endTime)
        {
            yield return new WaitForSeconds(endTime);

            activeAudio.Stop();
            ++currentTrack;
            PlayNextTrack();
        }

        public void ChangeMusicGroup(int newMusicGroup)
        {
            if (newMusicGroup >= MusicGroups.Count)
            {
                Debug.LogWarning("No music group with that ID!");
                return;
            }

            currentGroup = newMusicGroup;
        }


        [Serializable]
        public class MusicGroup
        {
            public List<AudioSource> MusicTracks = new List<AudioSource>();
        }

        private void OnDestroy()
        {
            GameSettings.OnSettingsChanged -= ToggleMusic;
        }
    }
}