using System;
using System.Collections.Generic;
using Code.Audio.EnumTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Audio
{
	public class SoundLibrary : MonoBehaviour
	{
		[SerializeField] private SoundGroup[] soundGroups;

		private Dictionary<SoundType, AudioClip[]> groupDictionary = new Dictionary<SoundType, AudioClip[]>();

		protected void Awake()
		{
			foreach (SoundGroup soundGroup in soundGroups) 
				groupDictionary.Add(soundGroup.GroupID, soundGroup.Clips);
		}

		public AudioClip GetClipFromName(SoundType clipName)
		{
			if (groupDictionary.ContainsKey(clipName))
			{
				AudioClip[] sounds = groupDictionary[clipName];
				return sounds[Random.Range(0, sounds.Length)];
			}

			return null;
		}

		[Serializable]
		public class SoundGroup
		{
			public string Name;
			public SoundType GroupID;
			public AudioClip[] Clips;

			public SoundGroup()
			{
				Name = GroupID.ToString();
			}
		}
	}
}