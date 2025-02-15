using System;
using UnityEngine;

namespace Assets._Project.Scripts.Core.UserInput
{
	public class InputHandler : MonoBehaviour
	{
		public event Action<Vector2> OnRightMouseDown;
		public event Action<Vector2> OnLeftMouseDown;
		public event Action<Vector2> OnLeftMouseUp;
		public event Action<Vector2> OnLeftMouseDrag;

		public Vector3 MousePosition => Input.mousePosition;

		private void Update()
		{

			//Detect right mouse
			if (Input.GetMouseButtonDown((int)MouseButton.Right))
			{
				OnRightMouseDown?.Invoke(Input.mousePosition);
			}


			//Detect left mouse
			if (Input.GetMouseButtonDown((int)MouseButton.Left))
			{
				OnLeftMouseDown?.Invoke(Input.mousePosition);
			}

			if (Input.GetMouseButton((int)MouseButton.Left))
			{
				OnLeftMouseDrag?.Invoke(Input.mousePosition);
			}

			if (Input.GetMouseButtonUp((int)MouseButton.Left))
			{
				OnLeftMouseUp?.Invoke(Input.mousePosition);
			}
		}

		public enum MouseButton
		{
			Left = 0,
			Right = 1,
			Middle = 2
		}
	}
}
