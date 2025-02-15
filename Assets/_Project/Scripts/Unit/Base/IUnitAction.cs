using UnityEngine;

namespace Assets._Project.Scripts.Unit.Base
{
	public interface IUnitAction
	{

	}

	public interface IUnitMoveable : IUnitAction
	{
		public void MoveToPosition(Vector3 position);
	}

}