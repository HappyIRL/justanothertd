using UnityEngine;

namespace Assets._Project.Scripts.Core.CameraScripts
{
	public class CameraController : MonoBehaviour
    {

        private Camera camera;

        void Awake()
        {
            camera = GetComponent<Camera>();
        }

		public bool GetRaycastHitFromScreenPoint(Vector3 screenPoint, out RaycastHit hit)
		{
			Ray ray = camera.ScreenPointToRay(screenPoint);
			return Physics.Raycast(ray, out hit, Mathf.Infinity);
		}

		public bool GetRaycastHitFromScreenPoint(Vector3 screenPoint, out RaycastHit hit, LayerMask layerMask)
		{
			Ray ray = camera.ScreenPointToRay(screenPoint);
			return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
		}

		public Vector3 GetWorldPositionAtDepth(Vector2 screenPoint, float depth)
        {
            Ray ray = camera.ScreenPointToRay(screenPoint);

            Vector3 worldPosition = ray.origin + ray.direction * (depth - ray.origin.y) / ray.direction.y;

            return worldPosition;
        }

        public bool GetWorldPositionFromScreenPoint(Vector2 screenPoint, out Ray ray)
        {
            ray = camera.ScreenPointToRay(screenPoint);
            return true; // Always returns true since we don't rely on raycast hits here
        }

        public bool GetWorldPositionFromScreenPoint(Vector3 screenPoint, LayerMask layerMask, out Vector3 worldPosition)
        {
            Ray ray = camera.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                worldPosition = hit.point;
                return true;
            }

            worldPosition = Vector3.zero;
            return false;
        }
    }
}
