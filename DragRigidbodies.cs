//https://www.youtube.com/watch?v=jdNXiFVSMJg
//https://www.youtube.com/watch?v=bCL78Erkuk4

using UnityEngine;
using System.Collections;

public class DragRigidbodies : MonoBehaviour {
	public float speed = 15;

	private bool drag = false;
	private Transform cam;
	private Transform dragMark;
	private float startSpeed;
	private Rigidbody rigid;

	bool canPickUp = false;

	private Rigidbody hold;
	public Vector3 puloc;
	public GameObject player;
	public bool examining = false;

	private Quaternion initialRot;
	private Quaternion mouseRot;

	void Start () {
		//for the crosshair
		if (GameObject.Find ("DragMark"))
			dragMark = GameObject.Find ("DragMark").transform;
		else
			Debug.LogError ("No DragMark found. Make sure there's an empty GameObject called DragMark that's attached to the Main Camera");

		if (Camera.main != null)
			cam = Camera.main.transform;
		else
			Debug.LogError ("No Main Camera found. Make sure there's a camera tagged as MainCamera");

		startSpeed = speed;
	}
	
	void Update () {
		//if there isn't dialogue going on, you can pick things up
		if (!gameObject.GetComponent<Dialogue> ().running) {
			if (dragMark == null || cam == null)
				return;
			//shoot a ray to see if it hits something
			Ray ray = Camera.main.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2));
			RaycastHit hit;
			//if it hits, check if it's a rigidbody, and if it is, you can pick it up and show possibility of dialogue
			if (Physics.Raycast (ray, out hit, 2) && hit.collider.gameObject.GetComponent ("Rigidbody") != null) {
				canPickUp = true;
				if (hit.collider.gameObject.GetComponent<TextAppear> () != null) {
					hit.collider.gameObject.GetComponent<TextAppear> ().canShow = true;
				}
			} else {
				canPickUp = false;
			}

			//if you hold these two inputs down, you can rotate the rigidbody
			if (Input.GetMouseButton (1) && Input.GetMouseButton (0)) {
				if (Physics.Raycast (ray, out hit, 2)) {
					if (hit.collider.GetComponent<Rigidbody> () != null) {
						examining = true;
						hold = hit.collider.GetComponent<Rigidbody> ();
					}
				}
			} else if (Input.GetMouseButtonUp (1) && examining == true) {
				hold.useGravity = true;
				hold.transform.parent = null;
				examining = false;
			}

			if (examining == true && Input.GetMouseButton (0)) {
				player = GameObject.Find ("Player");
				hold.transform.parent = player.transform;
				speed = 0;
				hold.transform.Rotate (Input.GetAxis ("Mouse Y") * -1.25f, Input.GetAxis ("Mouse X") * -1.25f, Input.GetAxis ("Mouse Y") * -1.25f);
			}
			//if you hold down this input, you can drag around the rigidbody
			if (Input.GetMouseButton (0)) {
				canPickUp = false;
				if (Input.GetMouseButtonDown (0))
				if (Physics.Raycast (ray, out hit, 2)) {
					if (hit.collider.GetComponent<Rigidbody> () != null) {
						rigid = hit.collider.GetComponent<Rigidbody> ();
						dragMark.position = rigid.position;
						rigid.constraints = RigidbodyConstraints.FreezeRotation;
						drag = true;
					}
				}
			}

			if (Input.GetMouseButtonUp (0) && rigid) {
				rigid.constraints = RigidbodyConstraints.None;
				drag = false;
			}

			if (rigid == null || drag == false)
				return;


			float dis = Vector3.Distance (dragMark.position, rigid.position);
			dis = Mathf.Clamp (dis, 0, 3);
			speed = startSpeed * dis;

			Vector3 direction = (dragMark.position - rigid.position).normalized;
			rigid.velocity = direction * speed;


		}
	}

	void OnGUI () {
        //the crosshair
        GUI.color = Color.white;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		//GUI.Label (new Rect (Screen.width/2, Screen.height/2.2f, 100, 100), "X");
		
		if (canPickUp == true){
			//show that you can pick the object up
			//GUI.Label (new Rect (Screen.width/2 + 10, Screen.height/2.2f + 10, 100, 100), "Examine");
			
		}
	}
}
