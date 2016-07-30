using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour {

	//how fast the player moves
	public float movementSpeed = 6.0f;
	//how sensitive the camera rotation is
	public float mouseSensitivity = 1.25f;
	float verticalRotation = 0;
	//cap of vertical camera movement
	public float upDownRange = 60.0f;

	float verticalVelocity = 0;
	public float jumpSpeed = 1.0f;

	CharacterController characterController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		//run the start dialogue when the game begins
		gameObject.GetComponent<Dialogue> ().Run (gameObject.GetComponent<DialogueImplementation>().defaultDialogue);
	}
	
	// Update is called once per frame
	void Update () {
		//if there isn't dialogue going on, you can move around
		if (!gameObject.GetComponent<Dialogue> ().running) {
			//hide cursor
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			//rotation of camera
			if (gameObject.GetComponent<DragRigidbodies> ().examining == false) {
				float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
				transform.Rotate (0, rotLeftRight, 0);

				verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
				verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
				Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
			}

			//movement -- set keymap under Edit > Project Settings > Input
			float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
			float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

			verticalVelocity += Physics.gravity.y * Time.deltaTime;

			if (characterController.isGrounded && Input.GetButtonDown ("Jump")) {
				verticalVelocity = jumpSpeed;
			}

			Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);

			speed = transform.rotation * speed;

			characterController.Move (speed * Time.deltaTime);
	
		}
	}
	
}
