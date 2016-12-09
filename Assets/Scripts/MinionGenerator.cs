using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGenerator : MonoBehaviour
{
	
	private float spawnInterval = 10f;

	public GameObject enemyMinionPrefab;

	public void Start()
	{
		InvokeRepeating("spawnNewMinion", 0f, spawnInterval);
	}

	public static int RangeConst = 1;

	public void Update()
	{
		Collider[] collide = Physics.OverlapSphere (this.transform.position, RangeConst);


		bool ok = false;
		foreach (Collider c in collide) {
			RobotController rc = c.GetComponentInParent<RobotController> ();
			if(rc != null)
				ok = true; 
		}

		if (!ok) {
			//GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("");
			return;
		}


		//GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("Press \'k\' to kill the generator");
		if (Input.GetKeyDown (KeyCode.K)) {
			Destroy (gameObject);
			DocumentClass.noGenerators--;
		}
	}

	private void spawnNewMinion()
	{
		//Random rnd = new Random ();
		GameObject enemyMinion = (GameObject) Instantiate(enemyMinionPrefab, transform.position + new Vector3(Random.Range(-2f,2f), 0, Random.Range(-2f,2f)) , transform.rotation);
		enemyMinion.transform.parent = GetComponentInParent<DocumentClass> ().transform;
	}
}
