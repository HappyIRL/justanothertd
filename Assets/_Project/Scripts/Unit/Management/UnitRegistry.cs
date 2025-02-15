using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Scripts.Unit.Selection;

namespace Assets._Project.Scripts.Unit.Management
{
    public class UnitRegistry : MonoBehaviour
    {
        private List<ISelectable> allUnits = new List<ISelectable>();

        public void RegisterUnit(ISelectable unit)
        {
            if (!allUnits.Contains(unit))
            {
                allUnits.Add(unit);
            }
        }

        public void DeregisterUnit(ISelectable unit)
        {
            if (allUnits.Contains(unit))
            {
                allUnits.Remove(unit);
            }
        }

        public List<ISelectable> GetAllUnits()
        {
            return allUnits;
        }
    }
}