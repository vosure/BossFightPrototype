using System;
using UnityEngine;

namespace Code.Audio
{
    [Serializable]
    public class Track
    {
        public AudioClip clip;
        public float volume;

        public Track(AudioClip clip, float volume = 1f)
        {
            this.clip = clip;
            this.volume = volume;
        }
    }
}