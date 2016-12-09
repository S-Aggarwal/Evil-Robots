using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour, IComparable<RobotController> {

	public int CompareTo(RobotController other) {
		//Debug.Log((" "+id) + " " + other.id);

		if (id < other.id)
			return -1;
		if (id > other.id)
			return 1;
		return 0;
	}

	UnityEngine.AI.NavMeshAgent navMesh;

	GameObject healthBar;
	HealthBar hbController;

	public static string TAG = "Player";
	public GameObject HealthBarPrefab;

	public bool isActive;

	public GameObject bulletPrefab;
	public static int idv = 0;
	public int id;

	// Use this for initialization
	void Start () {
		id = RobotController.idv++;

		navMesh = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		//DocumentClass.activeRobots.Add(this,true);
		healthBar = (GameObject)Instantiate (HealthBarPrefab, transform.position, Quaternion.identity);
		hbController = healthBar.GetComponentInChildren<HealthBar> ();
	}

	Vector3 target;
	EnemyController targetEnemy;
	double lastAttack = 0;
	public static double AttackTimeConst = 0.75;
	public static int AttackRangeConst = 2;
	public static int AttackHitConst = 10;
	public static int AttackObserveConst = 8;

	void minionAttack()
	{
		lastAttack += Time.deltaTime;

		Collider[] collide = Physics.OverlapSphere (this.transform.position, AttackObserveConst);
		double maxprior = 0.0;
		EnemyController maxp = null;

		foreach (Collider c in collide) {
			EnemyController rc = c.GetComponentInParent<EnemyController> ();
			if (rc == null)
				continue;
			Vector3 v = (transform.position - rc.transform.position);
			double prior = 1.0 / (v.magnitude==0?0.0001:v.magnitude);


			if (prior > maxprior) {
				maxprior = prior;
				maxp = rc;
			}
		}

		targetEnemy = maxp;

		/*if (target != null) {
			Debug.Log (target.name);
		}*/

		if (targetEnemy != null) {
			navMesh.SetDestination (targetEnemy.transform.position);

			Vector3 v = this.transform.position - targetEnemy.transform.position;
			if ((lastAttack >= AttackTimeConst) && (v.magnitude <= AttackRangeConst)) {
				targetEnemy.HP -= AttackHitConst;

				if (targetEnemy.HP <= 0) {
					Debug.Log ("You die!!!!");
					targetEnemy.kill();
				} else {
				}

				lastAttack = 0;
			}
		}
	}

	public void setActive()
	{
		isActive = true;
		if(!DocumentClass.activeRobots.ContainsKey(this)) DocumentClass.activeRobots.Add(this,true);
	}

	public void setPassiveDontRemove() {
		isActive = false;
	}

	public void setPassive()
	{
		DocumentClass.activeRobots.Remove (this);
		isActive = false;
	}

	public void setTarget(Vector3 newTarget)
	{
		target = newTarget;
		targetEnemy = null;
	}

	public void setTargetEnemy(EnemyController enemy)
	{
		targetEnemy = enemy;
	}

	public int HP = 100;

	public static int kills = 0;

	public void kill()
	{
		++kills;
		Destroy (healthBar);
		Destroy (gameObject);
	}

	public int ShootThreshold = 20;

	public int DamageToEnemies = 3;

	void Shoot ()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(targetEnemy.transform);
	}
		
	// Update is called once per frame
	void Update () {
		if (!isActive) {
			minionAttack ();
		} else if (targetEnemy) {
			if ((transform.position - targetEnemy.transform.position).magnitude <= ShootThreshold) {
				targetEnemy.HP -= DamageToEnemies;
				Shoot ();
				if (targetEnemy.HP <= 0) {
					targetEnemy.killedByHero ();
				}
			} else {
				navMesh.SetDestination (targetEnemy.transform.position);
			}
		} else {
			navMesh.SetDestination (target);
		}
		healthBar.transform.position = transform.position;
		hbController.setHealth ((float)HP / 100f);
	}
}
