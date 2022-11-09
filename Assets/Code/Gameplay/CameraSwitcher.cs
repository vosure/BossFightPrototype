using Cinemachine;
using Code.Infrastructure;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay
{
    public class CameraSwitcher : MonoBehaviour
    {
        private const int TopPriority = 100;
        private const int LowestPriority = 0;
    
        [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
        [SerializeField] private CinemachineVirtualCamera playerAimCamera;
    
        private InputService _input;

        private void Start() => 
            InitializeFields();

        private void OnDisable() => 
            Unsubscribe();

        private void InitializeFields()
        {
            _input = ServiceLocator.Instance.InputService;
        
            playerFollowCamera.Priority = TopPriority;
            playerAimCamera.Priority = LowestPriority;

            Subscribe();
        }

        private void SetFollowCameraAsMain()
        {
            playerFollowCamera.Priority = TopPriority;
            playerAimCamera.Priority = LowestPriority;
        }

        private void SetAimCameraAsMain()
        {
            playerAimCamera.Priority = TopPriority;
            playerFollowCamera.Priority = LowestPriority;
        }
    
        private void Subscribe()
        {
            _input.OnAimStarted += SetAimCameraAsMain;
            _input.OnAimEnded += SetFollowCameraAsMain;
        }

        private void Unsubscribe()
        {
            _input.OnAimStarted -= SetAimCameraAsMain;
            _input.OnAimEnded -= SetFollowCameraAsMain;
        }
    }
}
