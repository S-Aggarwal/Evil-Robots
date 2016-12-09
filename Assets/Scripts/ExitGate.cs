using UnityEngine;

public class ExitGate : MonoBehaviour
{
	private bool unlocked;
	public static string tag = "Hero";

	private void Start()
	{
		unlocked = false;
	}

	public static int RangeConst = 1;

	public bool finished = false;

	public void Update()
	{
		unlocked = DocumentClass.noGenerators == 0;

		Collider[] collide = Physics.OverlapSphere (this.transform.position, RangeConst);

		bool ok = false;
		foreach (Collider c in collide) {
			RobotController rc = c.GetComponentInParent<RobotController> ();
			if(rc != null)
				ok = true; 
		}

		if (!ok) return;

		Debug.Log("gen: " +DocumentClass.noGenerators);

		if (unlocked)
				{
		GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("You won!");
//Debug.Log(1/(DocumentClass.noGenerators));
				}
	}


}
