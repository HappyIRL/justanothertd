using UnityEngine;
using Assets._Project.Scripts.Unit.Base;
using Assets._Project.Scripts.Unit.Visual;

namespace Assets._Project.Scripts.Unit.Selection
{
	[RequireComponent(typeof(GameUnit))]
	[RequireComponent(typeof(UnitColorHandler))]
	public class UnitSelectionVisual : MonoBehaviour
	{
		[SerializeField] private Color selectedColor = Color.green;

		private int selectionColorId = -1;
		private GameUnit unit;
		private UnitColorHandler unitColorHandler;

		private void Awake()
		{
			Initialize(GetComponent<GameUnit>(), GetComponent<UnitColorHandler>());
		}

		public void Initialize(GameUnit unit, UnitColorHandler unitColorHandler)
		{
			this.unit = unit;
			this.unitColorHandler = unitColorHandler;
			unit.OnSelectionChanged += HandleSelection;
		}

		private void HandleSelection(bool isSelected)
		{
			if (unitColorHandler != null)
			{
				if (isSelected)
					selectionColorId = unitColorHandler.PushColor(selectedColor, ColorPriority.Select);
				else if (selectionColorId != -1)
					unitColorHandler.PopColor(selectionColorId);
			}
		}

		private void OnDisable()
		{
			unit.OnSelectionChanged -= HandleSelection;
		}
	}
}