using Code.Audio;
using Code.Infrastructure.Factories;
using Code.Utility;
using UnityEngine;

namespace Code.Infrastructure.Services
{
	public class ServiceLocator : Singleton<ServiceLocator>
	{
		[SerializeField] private InputService inputService;
		[SerializeField] private StaticDataService staticDataService;
		
		[SerializeField] private VFXFactory vfxFactory;
		[SerializeField] private GameFactory gameFactory;

		[SerializeField] private PlayerProgressService playerProgress;
		[SerializeField] private WindowService windowService;

		[SerializeField] private SceneLoaderService sceneLoaderService;
		
		[SerializeField] private AudioService audioService;
		[SerializeField] private MusicService musicService;

		public InputService InputService => inputService;
		public StaticDataService StaticDataService => staticDataService;
		public VFXFactory VfxFactory => vfxFactory;
		public GameFactory GameFactory => gameFactory;
		public PlayerProgressService PlayerProgress => playerProgress;
		public WindowService WindowService => windowService;

		public SceneLoaderService SceneLoaderService => sceneLoaderService;

		public AudioService AudioService => audioService;
		public MusicService MusicService => musicService;
	}
}