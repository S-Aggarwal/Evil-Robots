using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		heroMask = LayerMask.GetMask ("GoodCharacter");
		enemyMask = LayerMask.GetMask ("BadCharacter");
		floorMask = LayerMask.GetMask ("Floor");
	}

	float camRayLength = 2000f; 
	int floorMask;
	int heroMask;
	int enemyMask;
	const string LEFT_MOUSE_BUTTON = "Fire1";
	const string RIGHT_MOUSE_BUTTON = "Fire2";
	public float MinX = -130;
	public float MinZ = -130;
	public float MaxX = 100;
	public float MaxZ = 100;
	const float deltaScreenMove = 2f;
	const float screenEdgeThreshold = 10f;
	const float heroBottomThreshold = 0f;
	const float heroTopThreshold = 40f;
	const float heroLeftThreshold = -40f;
	const float heroRightThreshold = 40f;
	
	// Update is called once per frame
	void Update () {
		if (DocumentClass.activeRobots.Count == 0) {
			RobotController rob = GetComponentInParent<DocumentClass> ().GetComponentInChildren<RobotController> ();
			if (rob)
			{
					rob.setActive();
			}
			else
			{
				GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("Game over!");
				//TODO lose
			}
		}
		Vector3 mousepos = new Vector3(0,0,0);
		Vector2 mousexy = Input.mousePosition;
		if ((mousexy.x <= screenEdgeThreshold))// && (DocumentClass.activeRobot.transform.position.x <= transform.position.x + heroRightThreshold))
		{
			transform.position += new Vector3(-deltaScreenMove, 0, 0);
		}
		if ((mousexy.y <= screenEdgeThreshold))// && (DocumentClass.activeRobot.transform.position.z <= transform.position.z + heroTopThreshold))
		{
			transform.position += new Vector3(0, 0, -deltaScreenMove);
		}
		if ((mousexy.x >= Screen.width - screenEdgeThreshold))// && (DocumentClass.activeRobot.transform.position.x >= transform.position.x + heroLeftThreshold))
		{
			transform.position += new Vector3(deltaScreenMove, 0, 0);
		}
		if ((mousexy.y >= Screen.height - deltaScreenMove))// && (DocumentClass.activeRobot.transform.position.z >= transform.position.z + heroBottomThreshold))
		{
			transform.position += new Vector3(0, 0, deltaScreenMove);
		}
		Ray camRay = GetComponent<Camera>().ScreenPointToRay(mousexy);
		RaycastHit floorHit;
		if (Physics.Raycast(camRay, out floorHit, camRayLength, enemyMask) && Input.GetButton(RIGHT_MOUSE_BUTTON))
		{
			foreach (KeyValuePair<RobotController,bool> kvp in DocumentClass.activeRobots) {
				kvp.Key.setTargetEnemy (floorHit.transform.GetComponentInParent<EnemyController> ());
			}
		}
		else if (Physics.Raycast (camRay, out floorHit, camRayLength, heroMask) && Input.GetButton(LEFT_MOUSE_BUTTON)) {
			RobotController newActiveRobot = floorHit.transform.gameObject.GetComponentInParent<RobotController> ();
			if (newActiveRobot != null) {
				if(!Input.GetKey(KeyCode.X)) {
					foreach(KeyValuePair<RobotController,bool> kvp in DocumentClass.activeRobots) {
						kvp.Key.setPassiveDontRemove ();
					}
					DocumentClass.activeRobots.Clear ();
				}
				newActiveRobot.setActive ();
			}
		}
		else if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			mousepos = floorHit.point;
			mousepos.y = 0;
			if (Input.GetButton (RIGHT_MOUSE_BUTTON)) {
				foreach (KeyValuePair<RobotController,bool> kvp in DocumentClass.activeRobots) {
					kvp.Key.setTarget (mousepos);
				}
			}
		}
	}
}
