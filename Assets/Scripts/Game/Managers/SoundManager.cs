using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundFxDescription m_description = default;
        [SerializeField] private AudioSource m_audioPlayerPrefab = null;

        private List<AudioSource> m_audioPlayers = new List<AudioSource>();
        private Stack<AudioSource> m_freeAudioPlayers = new Stack<AudioSource>();

        private void OnEnable()
        {
            EventBus.Instance.AddListener<SoundEvents.SoundEvent>(OnSoundEvent);
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<SoundEvents.SoundEvent>(OnSoundEvent);
        }

        private void OnSoundEvent(SoundEvents.SoundEvent e)
        {
            var data = m_description.GetSoundData(e.type);
            PlaySound(data);
        }

        private void PlaySound(SoundFxDescription.SoundData data)
        {
            var audioPlayer = PrepareAudioPlayer();
            audioPlayer.volume = data.volume;
            audioPlayer.PlayOneShot(GetClip(data));
        }

        private AudioClip GetClip(SoundFxDescription.SoundData data)
        {
            return data.clips[0]; // TODO random?
        }

        private AudioSource PrepareAudioPlayer()
        {
            if (m_freeAudioPlayers.Count <= 0)
            {
                var newPlayer = CreateAudioPlayer();
                m_audioPlayers.Add(newPlayer);
                return newPlayer;
            }

            return m_freeAudioPlayers.Pop();
        }

        private AudioSource CreateAudioPlayer()
        {
            return Instantiate(m_audioPlayerPrefab);
        }

        private void Update()
        {
            // Super not efficient, but no one cares
            m_freeAudioPlayers.Clear();
            foreach (var player in m_audioPlayers)
                if (!player.isPlaying)
                    m_freeAudioPlayers.Push(player);
        }
    }
}
