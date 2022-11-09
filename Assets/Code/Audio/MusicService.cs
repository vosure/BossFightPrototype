using System;
using System.Collections;
using Code.Audio.EnumTypes;
using Code.Utility;
using UnityEngine;

namespace Code.Audio
{
	public class MusicService : MonoBehaviour
	{
		[SerializeField] private bool musicOn;
		[SerializeField] private Track[] menu;
		[SerializeField] private Track[] gameMusicTracks;

		private Coroutine _currentMusicCoroutine;
		private MusicType _currentMusicType = MusicType.None;
		private bool _isPlayingMusic;

		private int _currentTrackIndex;

		private AudioService _audioService;
		
		private void Awake()
		{
			_audioService = GetComponent<AudioService>();
		}

		public void PlayMusic(MusicType musicTypeToPlay)
		{
			if (_currentMusicType == musicTypeToPlay)
				return;

			StopMusicIfPlaying();

			_currentTrackIndex = 0;
			StartPlayingNewMusic(musicTypeToPlay);
		}

		private void StartPlayingNewMusic(MusicType musicType)
		{
			_isPlayingMusic = true;
			_currentMusicType = musicType;
			
			_currentMusicCoroutine = StartCoroutine(PlayMusicCoroutine(musicType));
		}

		private IEnumerator PlayMusicCoroutine(MusicType musicType)
		{
			Track[] tracksToPlay = GetTrackListByType(musicType);

			while (true)
				yield return SetUpMusicTrack(tracksToPlay, out _);

			object SetUpMusicTrack(Track[] tracks, out Track current)
			{
				current = tracks[_currentTrackIndex];
				_audioService.SoundtrackVolume = current.volume;
				_audioService.PlayMusic(current.clip);
				
				if (++_currentTrackIndex >= tracks.Length)
					_currentTrackIndex = 0;
				
				return new WaitForSeconds(current.clip.length - 4f);
			}
		}

		private void StopMusicIfPlaying()
		{
			if (_isPlayingMusic)
			{
				_audioService.StopMusic();
				StopCoroutine(_currentMusicCoroutine);

				_currentMusicCoroutine = null;
				_isPlayingMusic = false;
				_currentMusicType = MusicType.None;
			}
		}

		private Track[] GetTrackListByType(MusicType musicType)
		{
			if (musicType == MusicType.Game)
				return gameMusicTracks;
			if (musicType == MusicType.Menu)
				return menu;

			return menu;
		}
	}
}