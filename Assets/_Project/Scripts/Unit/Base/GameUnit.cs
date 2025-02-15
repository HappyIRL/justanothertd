using System;
using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Scripts.Unit.Management;
using Assets._Project.Scripts.Unit.Selection;

namespace Assets._Project.Scripts.Unit.Base
{
    [RequireComponent(typeof(Renderer))]
	public class GameUnit : MonoBehaviour, ISelectable
    {
		[SerializeField] private UnitRegistry unitRegistry;

		public Vector3 Position => transform.position;
        public IEnumerable<IUnitAction> UnitActions => unitActions;
        public event Action<bool> OnSelectionChanged;

        private List<IUnitAction> unitActions = new List<IUnitAction>();

        private void Awake()
        {
			Initialize(GetComponents<IUnitAction>());
        }

		public void Initialize(IEnumerable<IUnitAction> actions)
		{
			if (actions != null)
				unitActions.AddRange(actions);
		}

		private void OnEnable()
        {
            unitRegistry.RegisterUnit(this);
        }

        private void OnDisable()
        {
            unitRegistry.DeregisterUnit(this);
        }

		public void OnSelected()
        {
            OnSelectionChanged?.Invoke(true);
        }

        public void OnDeselected()
        {
            OnSelectionChanged?.Invoke(false);
        }
    }
}