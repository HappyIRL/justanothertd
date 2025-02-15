using System;
using UnityEngine;
using Assets._Project.Scripts.Core.CameraScripts;
using System.Collections.Generic;
using Assets._Project.Scripts.Core.UserInput;
using Assets._Project.Scripts.Unit.Management;

namespace Assets._Project.Scripts.Unit.Selection
{
	public interface IHoverable
	{
		void OnHoverEnter();
		void OnHoverExit();
	}

	public class UnitHoverSystem : MonoBehaviour
	{
		[SerializeField] private CameraController cameraController;
		[SerializeField] private InputHandler inputHandler;
		[SerializeField] private UnitRegistry unitRegistry;

		private IHoverable lastHoveredObject;
		private List<IHoverable> hoveredUnits = new List<IHoverable>();
		private Vector2 startDragPosition;

		private void Update()
		{
			HandleNormalHover();
		}

		private void OnEnable()
		{
			inputHandler.OnLeftMouseDown += HandleLeftMouseDown;
			inputHandler.OnLeftMouseDrag += HandleSelectionBoxDrag;
			inputHandler.OnLeftMouseUp += HandleSelectionBoxEnd;
		}

		private void OnDisable()
		{
			inputHandler.OnLeftMouseDown -= HandleLeftMouseDown;
			inputHandler.OnLeftMouseDrag -= HandleSelectionBoxDrag;
			inputHandler.OnLeftMouseUp -= HandleSelectionBoxEnd;
		}

		private void HandleNormalHover()
		{
			if (cameraController.GetRaycastHitFromScreenPoint(inputHandler.MousePosition, out RaycastHit hit))
			{
				if (hit.collider.TryGetComponent<IHoverable>(out IHoverable hoverable))
				{
					if (hoverable != lastHoveredObject)
					{
						lastHoveredObject?.OnHoverExit(); // Exit old hover
						lastHoveredObject = hoverable;
						lastHoveredObject.OnHoverEnter(); // Enter new hover
					}
				}
				else
				{
					lastHoveredObject?.OnHoverExit();
					lastHoveredObject = null;
				}
			}
			else if (lastHoveredObject != null)
			{
				lastHoveredObject.OnHoverExit();
				lastHoveredObject = null;
			}
		}

		private void HandleLeftMouseDown(Vector2 mousePosition)
		{
			startDragPosition = mousePosition;
			ClearSelectionBoxHover();
		}

		private void HandleSelectionBoxDrag(Vector2 mousePosition)
		{
			ApplySelectionBoxHover(startDragPosition, mousePosition);
		}

		private void HandleSelectionBoxEnd(Vector2 mousePosition)
		{
			ClearSelectionBoxHover();
		}

		private void ApplySelectionBoxHover(Vector2 startScreen, Vector2 endScreen)
		{
			Vector3 startWorld = cameraController.GetWorldPositionAtDepth(startScreen, 0f);
			Vector3 endWorld = cameraController.GetWorldPositionAtDepth(endScreen, 0f);

			Rect selectionBox = new Rect(
				Mathf.Min(startWorld.x, endWorld.x),
				Mathf.Min(startWorld.z, endWorld.z),
				Mathf.Abs(startWorld.x - endWorld.x),
				Mathf.Abs(startWorld.z - endWorld.z)
			);

			Debug.Log($"Updating Selection Box Hover. Selection Box: {selectionBox}");

			// Remove selection box hover from previous units
			foreach (var hovered in hoveredUnits)
			{
				hovered.OnHoverExit();
			}
			hoveredUnits.Clear();

			// Apply selection box hover to new units inside the selection box
			foreach (var selectable in unitRegistry.GetAllUnits())
			{
				if (selectable is MonoBehaviour mb && mb.TryGetComponent<Collider>(out Collider collider))
				{
					if (BoundsIntersectsRect(collider.bounds, selectionBox) && mb.TryGetComponent<UnitSelectionHover>(out UnitSelectionHover hoverable))
					{
						hoverable.OnHoverEnter();
						hoveredUnits.Add(hoverable);
					}
				}
			}
		}

		private void ClearSelectionBoxHover()
		{
			foreach (var hovered in hoveredUnits)
			{
				hovered.OnHoverExit();
			}
			hoveredUnits.Clear();
		}

		private bool BoundsIntersectsRect(Bounds bounds, Rect rect)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			return rect.Overlaps(new Rect(min.x, min.z, max.x - min.x, max.z - min.z));
		}
	}
}
