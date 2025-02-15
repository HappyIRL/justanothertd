using System;
using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Scripts.Unit.Base;
using Assets._Project.Scripts.Core.CameraScripts;
using Assets._Project.Scripts.Unit.Management;
using Assets._Project.Scripts.Core.UserInput;

namespace Assets._Project.Scripts.Unit.Selection
{
    public interface ISelectable
    {
        public void OnSelected();
        public void OnDeselected();
        public Vector3 Position { get; }

        IEnumerable<IUnitAction> UnitActions { get; }
    }

	public class UnitSelector : MonoBehaviour
	{
		public event Action<List<ISelectable>> OnSelectionChanged;

		[SerializeField] private InputHandler inputHandler;
		[SerializeField] private UnitRegistry unitRegistry;
		[SerializeField] private CameraController cameraController;

		private Vector2 startMousePosition;
		private List<ISelectable> selectedUnits = new List<ISelectable>();

		private void OnEnable()
		{
			inputHandler.OnLeftMouseDown += HandleLeftMouseDown;
			inputHandler.OnLeftMouseDrag += HandleLeftMouseDrag;
			inputHandler.OnLeftMouseUp += HandleLeftMouseUp;
		}

		private void OnDisable()
		{
			inputHandler.OnLeftMouseDown -= HandleLeftMouseDown;
			inputHandler.OnLeftMouseDrag -= HandleLeftMouseDrag;
			inputHandler.OnLeftMouseUp -= HandleLeftMouseUp;
		}

		private void HandleLeftMouseDown(Vector2 mousePosition)
		{
			startMousePosition = mousePosition;
		}

		private void HandleLeftMouseDrag(Vector2 mousePosition)
		{
			
		}

		private void HandleLeftMouseUp(Vector2 mousePosition)
		{
			SelectUnitsInBox(startMousePosition, mousePosition);
		}

		private void SelectUnitsInBox(Vector2 startScreen, Vector2 endScreen)
		{
			Vector3 startWorld = cameraController.GetWorldPositionAtDepth(startScreen, 0f);
			Vector3 endWorld = cameraController.GetWorldPositionAtDepth(endScreen, 0f);

			Rect selectionBox = new Rect(
				Mathf.Min(startWorld.x, endWorld.x),
				Mathf.Min(startWorld.z, endWorld.z),
				Mathf.Abs(startWorld.x - endWorld.x),
				Mathf.Abs(startWorld.z - endWorld.z)
			);

			// Clear previous selection
			foreach (var unit in selectedUnits)
			{
				unit.OnDeselected();
			}
			selectedUnits.Clear();

			// Iterate over all selectable units
			foreach (var selectable in unitRegistry.GetAllUnits())
			{
				if (selectable is MonoBehaviour mb && mb.TryGetComponent<Collider>(out Collider collider))
				{
					if (BoundsIntersectsRect(collider.bounds, selectionBox))
					{
						selectedUnits.Add(selectable);
						selectable.OnSelected();
					}
				}
			}
			OnSelectionChanged?.Invoke(selectedUnits);
		}

		private bool BoundsIntersectsRect(Bounds bounds, Rect rect)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			bool intersects = rect.Overlaps(new Rect(min.x, min.z, max.x - min.x, max.z - min.z));

			return intersects;
		}
	}

}