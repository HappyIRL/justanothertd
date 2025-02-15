using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Unit.Visual
{
	[RequireComponent(typeof(Renderer))]
	public class UnitColorHandler : MonoBehaviour
	{
		private Renderer unitRenderer;
		private List<(ColorPriority priority, int id, Color color)> colorOverrides = new List<(ColorPriority, int, Color)>();
		private int nextId = 1;

		private void Awake()
		{
			unitRenderer = GetComponent<Renderer>();
			if (unitRenderer != null)
			{
				colorOverrides.Add((ColorPriority.Default, 0, unitRenderer.material.color)); // Default color
			}
		}

		public int PushColor(Color newColor, ColorPriority priority)
		{
			if (unitRenderer != null)
			{
				int id = nextId++;

				// Remove any previous entry with the same ID
				colorOverrides.RemoveAll(entry => entry.id == id);

				// Insert based on priority index, ignoring fine order within the same priority
				int insertIndex = colorOverrides.FindIndex(entry => entry.priority > priority);
				if (insertIndex == -1)
				{
					colorOverrides.Add((priority, id, newColor));
				}
				else
				{
					colorOverrides.Insert(insertIndex, (priority, id, newColor));
				}

				ApplyTopColor();
				return id;
			}
			return -1;
		}

		public void PopColor(int id)
		{
			if (unitRenderer != null)
			{
				colorOverrides.RemoveAll(entry => entry.id == id);
				ApplyTopColor();
			}
		}

		private void ApplyTopColor()
		{
			if (colorOverrides.Count > 0)
			{
				unitRenderer.material.color = colorOverrides[0].color;
			}
		}
	}

	public enum ColorPriority
	{
		Hover,
		Select,
		Default
	}
}