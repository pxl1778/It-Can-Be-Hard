using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;
        [SerializeField]
        private float translateDamping;

        [SerializeField]
        private float rotation;

        // Use this for initialization
        void Start() {
        }

        private void Update()
        {
            rotation += CrossPlatformInputManager.GetAxis("Mouse X"); 
        }

        // Update is called once per frame
        void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			var wantedRotationAngle = rotation;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            Vector3 targetPosition = target.position;
			//transform.position = target.position;
			targetPosition -= currentRotation * Vector3.forward * distance;
            targetPosition = new Vector3(targetPosition.x, currentHeight, targetPosition.z);

            // Set the height of the camera
            transform.position = Vector3.Lerp(transform.position, targetPosition, translateDamping * Time.deltaTime);

			// Always look at the target
			transform.LookAt(target);
		}

        public void changeCameraLookTarget(string pTargetName)
        {
            Debug.Log("trying to change target: " + pTargetName);
            GameObject tTarget = GameObject.Find(pTargetName);
            if (tTarget != null)
            {
                Debug.Log("found target");
                target = tTarget.transform;
            }
        }
	}
}