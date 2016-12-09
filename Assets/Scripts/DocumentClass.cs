using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentClass : MonoBehaviour {

	public static SortedDictionary<RobotController,bool> activeRobots;
	public static int noGenerators = 4;

	// Use this for initialization
	void Start () {
		activeRobots = new SortedDictionary<RobotController, bool> ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
