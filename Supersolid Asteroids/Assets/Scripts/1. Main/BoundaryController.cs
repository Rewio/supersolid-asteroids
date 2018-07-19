using UnityEngine;

public abstract class BoundaryController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const float X_BOUNDARY = 8.89f;
	private const float Y_BOUNDARY = 5f;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Header("Base")]

	[SerializeField] private Camera mainCamera;
	[SerializeField] private Collider2D gameoCollider;
	[SerializeField] private GameObject gameoGameObject;

	//============================================================
	// Private Fields:
	//============================================================

	private Plane[] planes;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public virtual void Start() {

		// TODO: fix this 
		if (mainCamera == null) {
			mainCamera = Camera.main;
		}
		
		planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
	}

//	public void Start() {
//
//		float vertical   = mainCamera.orthographicSize;
//		float horizontal = vertical * Screen.width / Screen.height;
//
//		print(vertical + " - " + horizontal);
//
//	}

	public virtual void Update() {

		// if we are no-longer visible by the main camera...
		if (!GeometryUtility.TestPlanesAABB(planes, gameoCollider.bounds)) {

			print("Yes");

			// grab our current position so we can figure from which direction we exitted the screen.
			Vector2 currentPosition = gameoGameObject.transform.position;

			// if we left the screen horizontally, add or subtract twice the camera width to place us back in view.
			if (Mathf.Abs(currentPosition.x) > X_BOUNDARY) {
				currentPosition.x += (currentPosition.x > 0) ? -X_BOUNDARY * 2 : X_BOUNDARY * 2;
			}

			// the same as above, however this is if we have left the screen vertically.
			else if (Mathf.Abs(currentPosition.y) > Y_BOUNDARY) {
				currentPosition.y += (currentPosition.y > 0) ? -Y_BOUNDARY * 2 : Y_BOUNDARY * 2;
			}

			// finally update the game object with the new position.
			gameoGameObject.transform.position = currentPosition;
		}
	}

}
