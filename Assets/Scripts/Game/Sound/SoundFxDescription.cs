using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    [CreateAssetMenu(fileName = "SoundFxDescription", menuName = "Game/Sound Fx Description")]
    public class SoundFxDescription : ScriptableObject
    {
        [System.Serializable]
        public class SoundData
        {
            public SoundEvents.SoundType type = SoundEvents.SoundType.None;
            public List<AudioClip> clips = new List<AudioClip>();
            [Range(0.0f, 1.0f)] public float volume = 1.0f;
        }

        [SerializeField] private List<SoundData> m_soundData = new List<SoundData>();

        private Dictionary<SoundEvents.SoundType, SoundData> m_soundDataMap = new Dictionary<SoundEvents.SoundType, SoundData>();

        private void OnEnable()
        {
            foreach (var item in m_soundData)
                m_soundDataMap[item.type] = item;
        }

        public SoundData GetSoundData(SoundEvents.SoundType type)
        {
            return m_soundDataMap[type];
        }
    }
}
