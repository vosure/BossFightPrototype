using System.Collections;
using Code.Audio.EnumTypes;
using Code.Utility;
using UnityEngine;

namespace Code.Audio
{
	public class AudioService : MonoBehaviour
	{
		[SerializeField] [Range(0f, 1f)] private float volumePercent;
		[SerializeField] private float musicVolume;

		private bool MusicActive
		{
			get => _isMusicActive;
			set => _isMusicActive = value;
		}

		private bool SfxActive
		{
			get => _isSfxActive;
			set => _isSfxActive = value;
		}

		private bool _isMusicActive;
		private bool _isSfxActive;

		public float SoundtrackVolume { get; set; }

		private AudioSource _sfx2DSource;
		private AudioSource[] _musicSources;
		private AudioSource _loopedSound;
		private int _activeMusicSourceIndex;

		private SoundLibrary _library;

		protected void Awake()
		{
			_library = GetComponent<SoundLibrary>();

			Load();

			SoundtrackVolume = 1f;

			_musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++)
			{
				GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
				_musicSources[i] = newMusicSource.AddComponent<AudioSource>();
				_musicSources[i].volume = musicVolume;
				newMusicSource.transform.parent = transform;
			}

			GameObject loopedSource = new GameObject("Music Source " + 3);
			_loopedSound = loopedSource.AddComponent<AudioSource>();
			_loopedSound.volume = 0.45f;
			loopedSource.transform.parent = transform;

			GameObject newSfx2Dsource = new GameObject("2D Sfx Source");
			_sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
			newSfx2Dsource.transform.parent = transform;

			TurnMusicActive(MusicActive);
			TurnSfxActive(SfxActive);
		}

		public void PlayMusic(AudioClip clip, float fadeDuration = 1)
		{
			_activeMusicSourceIndex = 1 - _activeMusicSourceIndex;
			_musicSources[_activeMusicSourceIndex].clip = clip;
			_musicSources[_activeMusicSourceIndex].Play();

			StartCoroutine(AnimateMusicCrossFade(fadeDuration));
		}

		public void StopMusic()
		{
			foreach (AudioSource s in _musicSources)
			{
				s.Stop();
				s.loop = false;
				s.clip = null;
			}
		}

		public void PlayLoopedSound(AudioClip clip)
		{
			if (_loopedSound.isPlaying)
			{
				StopLoopedSound();
			}

			_loopedSound.loop = true;
			_loopedSound.clip = clip;
			_loopedSound.Play();
		}

		public void StopLoopedSound()
		{
			_loopedSound.Stop();
			_loopedSound.loop = false;
			_loopedSound.clip = null;
		}

		public void PlaySound(SoundType soundName, Vector3 pos)
		{
			if (SfxActive)
				PlaySound(_library.GetClipFromName(soundName), pos);
		}
		
		public void PlaySound(SoundType soundName, Vector3 pos, float volume)
		{
			if (SfxActive)
				PlaySound(_library.GetClipFromName(soundName), pos, volume);
		}

		private void PlaySound(AudioClip clip, Vector3 pos)
		{
			if (clip != null && SfxActive)
			{
				AudioSource.PlayClipAtPoint(clip, pos, volumePercent * 5);
			}
		}

		private void PlaySound(AudioClip clip, Vector3 pos, float volume)
		{
			if (clip != null && SfxActive)
			{
				AudioSource.PlayClipAtPoint(clip, pos, volumePercent * 5 * volume);
			}
		}

		public void PlaySound2D(SoundType soundName)
		{
			if (SfxActive)
				_sfx2DSource.PlayOneShot(_library.GetClipFromName(soundName), volumePercent);
		}

		public void PlaySound2D(SoundType soundName, float volume)
		{
			if (SfxActive)
				_sfx2DSource.PlayOneShot(_library.GetClipFromName(soundName), volumePercent * volume);
		}

		private IEnumerator AnimateMusicCrossFade(float duration)
		{
			float percent = 0;

			while (percent < 1)
			{
				percent += Time.deltaTime * 1 / duration;
				_musicSources[_activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolume * SoundtrackVolume, percent);
				_musicSources[1 - _activeMusicSourceIndex].volume =
					Mathf.Lerp(musicVolume * SoundtrackVolume, 0, percent);
				yield return null;
			}
		}

		public void ChangeVolumePercent(float newVolume)
		{
			volumePercent = newVolume;
		}

		public void TurnActive(bool active)
		{
			_sfx2DSource.mute = !active;
			_musicSources[0].mute = !active;
			_musicSources[1].mute = !active;
			_loopedSound.mute = !active;
			volumePercent = active ? 1f : 0f;
		}

		public void Mute()
		{
			_sfx2DSource.mute = true;
			_musicSources[0].mute = true;
			_musicSources[1].mute = true;
			_loopedSound.mute = true;
			volumePercent = 0f;
		}

		public void Unmute()
		{
			_sfx2DSource.mute = !SfxActive;
			_musicSources[0].mute = !MusicActive;
			_musicSources[1].mute = !MusicActive;
			_loopedSound.mute = !SfxActive;
			volumePercent = 1f;
		}

		private void TurnMusicActive(bool active)
		{
			foreach (AudioSource source in _musicSources)
			{
				source.mute = !active;
			}

			MusicActive = active;
		}

		private void TurnSfxActive(bool active)
		{
			SfxActive = active;
			if (active)
			{
				_sfx2DSource.mute = !active;
			}

			_loopedSound.mute = !active;
		}

		private void Save()
		{
			PlayerPrefs.SetInt("MusicOn", _isMusicActive ? 1 : 0);
			PlayerPrefs.SetInt("SfxOn", _isSfxActive ? 1 : 0);
		}

		private void Load()
		{
			MusicActive = PlayerPrefs.GetInt("MusicOn", 1) == 1;
			SfxActive = PlayerPrefs.GetInt("SfxOn", 1) == 1;
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				Save();
			}
		}

		private void OnApplicationQuit()
		{
			Save();
		}
	}
}