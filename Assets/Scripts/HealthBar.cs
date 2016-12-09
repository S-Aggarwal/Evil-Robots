using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public void setHealth(float hp)
	{
		transform.localPosition = new Vector3 (0, 5, hp / 2);
		transform.localScale = new Vector3 (0.1f, 0.1f, hp);
	}

}
