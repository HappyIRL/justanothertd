using UnityEngine;
using UnityEngine.AI;
using Assets._Project.Scripts.Unit.Base;

namespace Assets._Project.Scripts.Unit.Movement
{

	[RequireComponent(typeof(NavMeshAgent))]
	public class UnitMover : MonoBehaviour, IUnitMoveable
	{
		[SerializeField] private float moveSpeed = 10f;

		private NavMeshAgent agent;

		private void Awake()
		{
			Initialize(GetComponent<NavMeshAgent>());
		}

		public void Initialize(NavMeshAgent agent)
		{
			this.agent = agent;

			if (agent != null)
				agent.speed = moveSpeed;
		}

		public void MoveToPosition(Vector3 position)
		{
			if (agent != null && agent.isOnNavMesh)
			{
				agent.SetDestination(position);
			}
		}

		public bool HasReachedDestination()
		{
			return agent != null && agent.remainingDistance <= agent.stoppingDistance;
		}
	}
}
