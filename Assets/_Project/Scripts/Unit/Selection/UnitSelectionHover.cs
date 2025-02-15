using Assets._Project.Scripts.Core.UserInput;
using Assets._Project.Scripts.Unit.Visual;
using UnityEngine;

namespace Assets._Project.Scripts.Unit.Selection
{
	[RequireComponent(typeof(UnitColorHandler))]
	public class UnitSelectionHover : MonoBehaviour, IHoverable
	{
		[SerializeField] private Color hoverColor = Color.yellow;
		
		private UnitColorHandler colorHandler;
		private int hoverColorId = -1;

		private void Awake()
		{
			colorHandler = GetComponent<UnitColorHandler>();
		}

		public void OnHoverEnter()
		{
			if (colorHandler != null && hoverColorId == -1)
			{
				hoverColorId = colorHandler.PushColor(hoverColor, ColorPriority.Hover);
			}
		}

		public void OnHoverExit()
		{
			if (colorHandler != null && hoverColorId != -1)
			{
				colorHandler.PopColor(hoverColorId);
				hoverColorId = -1;
			}
		}
	}
}