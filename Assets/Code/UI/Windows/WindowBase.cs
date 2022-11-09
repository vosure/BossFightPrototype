using System;
using Code.Infrastructure;
using Code.Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
	public abstract class WindowBase : MonoBehaviour
	{
		[SerializeField] private WindowType windowType;
		[SerializeField] private Button closeButton;

		public WindowType WindowType => windowType;

		private InputService _inputService;
		
		protected void Awake()
		{
			OnAwake();
			
			SetListeners();
			closeButton?.onClick.AddListener(Close);
		}
		
		public void Open()
		{
			_inputService = ServiceLocator.Instance.InputService;
			
			OnOpen();
			
			gameObject.SetActive(true);
		}

		public void Close()
		{
			OnClose();

			gameObject.SetActive(false);
		}

		protected void DestroyWindow() => 
			Destroy(gameObject);

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnOpen()
		{
			_inputService.DisableInput();
		}

		protected virtual void OnClose()
		{
			_inputService.EnableInput();
		}

		protected abstract void SetListeners();

		protected abstract void RemoveListeners();
	}
}