using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform target;
	private RobotController targetEnemy;

	[Header("General")]

	public float range = 15f;

	[Header("Use Bullets (default)")]
	public GameObject bulletPrefab;
	public float fireRate = 2f;
	private float fireCountdown = 0f;

	[Header("Unity Setup Fields")]

	public string enemyTag = "Player";

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<RobotController>();
		} else
		{
			target = null;
		}

	}

	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			return;
		}

		if (fireCountdown <= 0f)
		{
			Shoot();
			fireCountdown = fireRate;
		}
		fireCountdown -= Time.deltaTime;
	}


	void Shoot ()
	{
		if (targetEnemy) {
			targetEnemy.HP = 0;
			targetEnemy.kill ();

			GameObject bulletGO = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
			Bullet bullet = bulletGO.GetComponent<Bullet> ();

			if (bullet != null)
				bullet.Seek (target);
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}