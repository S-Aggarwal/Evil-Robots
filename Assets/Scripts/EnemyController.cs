using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public static string tag = "EnemyMinion";
	public static double HERO_ATTACK_PRIORITY = 1.3;

	UnityEngine.AI.NavMeshAgent navMesh;

	GameObject healthBar;
	HealthBar hbController;
	public GameObject HealthBarPrefab;

	public int HP = 100;

	// Use this for initialization
	void Start () {
		navMesh = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		healthBar = (GameObject)Instantiate (HealthBarPrefab, transform.position, Quaternion.identity);
		hbController = healthBar.GetComponentInChildren<HealthBar> ();
	}

	RobotController target;

	double lastAttack = 0;
	public static double AttackTimeConst = 0.75;
	public static int AttackRangeConst = 2;
	public static int AttackHitConst = 10;
	public static int AttackObserveConst = 8;

	// Update is called once per frame
	void Update () {
		lastAttack += Time.deltaTime;

		Collider[] collide = Physics.OverlapSphere (this.transform.position, AttackObserveConst);
		double maxprior = 0.0;
		RobotController maxp = null;

		foreach (Collider c in collide) {
			RobotController rc = c.GetComponentInParent<RobotController> ();
			if (rc == null)
				continue;
			Vector3 v = (this.transform.position - rc.transform.position);
			double prior = 1.0 / (v.magnitude==0?0.0001:v.magnitude);

			if (DocumentClass.activeRobots.ContainsKey(rc))
				prior *= HERO_ATTACK_PRIORITY;

			if (prior > maxprior) {
				maxprior = prior;
				maxp = rc;
			}
		}

		target = maxp;

		/*if (target != null) {
			Debug.Log (target.name);
		}*/

		if (target != null) {
			navMesh.SetDestination (target.transform.position);

			Vector3 v = this.transform.position - target.transform.position;
			if ((lastAttack >= AttackTimeConst) && (v.magnitude <= AttackRangeConst)) {
				target.HP -= AttackHitConst;

				if (target.HP <= 0) {
					target.kill();
				} 

				lastAttack = 0;
			}
		}
		healthBar.transform.position = transform.position;
		hbController.setHealth ((float)HP / 100f);

	}

	public void kill() {
		DestroyImmediate (healthBar);
		DestroyImmediate (this.gameObject);
	}

	public GameObject RobotControllerPrefab;
	public void killedByHero() {
		((GameObject) Instantiate(RobotControllerPrefab,this.transform.position,this.transform.rotation)).transform.parent = GetComponentInParent<DocumentClass>().transform;
		kill ();
	}
}
