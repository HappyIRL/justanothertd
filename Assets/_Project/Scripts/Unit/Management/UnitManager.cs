using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Scripts.Unit.Base;
using Assets._Project.Scripts.Core.CameraScripts;
using Assets._Project.Scripts.Unit.Selection;
using Assets._Project.Scripts.Core.UserInput;

namespace Assets._Project.Scripts.Unit.Management
{
	public class UnitManager : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private UnitSelector unitSelector;

        private List<ISelectable> selectedUnits = new List<ISelectable>();

        private void OnEnable()
        {
            inputHandler.OnRightMouseDown += HandleRightMouseDown;
            unitSelector.OnSelectionChanged += UpdateSelectedUnits;
        }

        private void OnDisable()
        {
            inputHandler.OnRightMouseDown -= HandleRightMouseDown;
            unitSelector.OnSelectionChanged -= UpdateSelectedUnits;
        }

        private void HandleRightMouseDown(Vector2 screenPoint)
        {
            if (cameraController.GetWorldPositionFromScreenPoint(screenPoint, groundLayer, out Vector3 worldPosition))
            {
                MoveSelectedUnits(worldPosition);
            }
        }

        private void UpdateSelectedUnits(List<ISelectable> units)
        {
            selectedUnits = units;
        }

        private void MoveSelectedUnits(Vector3 worldPositon)
        {
            foreach (ISelectable selected in selectedUnits)
            {
                foreach (var action in selected.UnitActions)
                {
                    if (action is IUnitMoveable mover)
                    {
                        mover.MoveToPosition(worldPositon);
                        break;
                    }
                }
            }
        }
    }

}