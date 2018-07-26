using UnityEngine;

public class BoundaryController : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[Header("Base")]

	[SerializeField] private Collider2D objectCollider;
	[SerializeField] private Transform objectTransform;

	[SerializeField] private float objectHeight;
	[SerializeField] private float objectWidth;

	//============================================================
	// Private Fields:
	//============================================================

	private Camera mainCamera;

	private float cameraHeight;
	private float cameraWidth;

	private Plane[] planes;

	private Vector2 currentPosition;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public virtual void Start() {
		mainCamera = Camera.main;

		cameraHeight = mainCamera.orthographicSize;
		cameraWidth  = cameraHeight * Screen.width / Screen.height;

		planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
	}

	public virtual void Update() {

		// check if the object is no-longer visible by the camera
		if (!GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds)) {

			// grab the current position to check which side of the screen we left on
			currentPosition = objectTransform.position;

			// if our x position is greater than the camera width, we left horizontally
			if (Mathf.Abs(currentPosition.x) > cameraWidth) {
				currentPosition.x += (currentPosition.x > 0) ? (-cameraWidth * 2) - objectWidth : (cameraWidth * 2) + objectWidth;
			}

			// if it was the y position, we left vertically
			else if (Mathf.Abs(currentPosition.y) > cameraHeight) {
				currentPosition.y += (currentPosition.y > 0) ? (-cameraHeight * 2) - objectHeight : (cameraHeight * 2) + objectHeight;
			}

			// overwrite our objects position with it's new position
			objectTransform.position = currentPosition;
		}
	}

}
