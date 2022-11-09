using Code.Audio.EnumTypes;
using UnityEngine;

namespace Code.Audio
{
	public class PlayLoopedSound : MonoBehaviour
	{
		[SerializeField] private SoundType soundType;

		private AudioService _audioService;

		private void OnEnable()
		{
			AudioClip sound = FindObjectOfType<SoundLibrary>().GetClipFromName(soundType);

			_audioService = GetComponent<AudioService>();
			_audioService.PlayLoopedSound(sound);
		}

		private void OnDisable() => 
			_audioService.StopLoopedSound();
	}
}