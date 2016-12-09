using UnityEngine;

public class EntryGate : MonoBehaviour
{
	public GameObject heroPrefab;
	public int levelNum = 1;

	public static double ScreenTimeConst = 2.0;

	public void Start()
	{
		GameObject hero = (GameObject) Instantiate(heroPrefab, transform.position, transform.rotation);
		hero.transform.parent = GetComponentInParent<DocumentClass> ().gameObject.transform;


		GameObject hero2 = (GameObject) Instantiate(heroPrefab, transform.position, transform.rotation);
		hero2.transform.parent = GetComponentInParent<DocumentClass> ().gameObject.transform;
		GameObject hero3 = (GameObject) Instantiate(heroPrefab, transform.position, transform.rotation);
		hero3.transform.parent = GetComponentInParent<DocumentClass> ().gameObject.transform;
		GameObject hero4 = (GameObject) Instantiate(heroPrefab, transform.position, transform.rotation);
		hero4.transform.parent = GetComponentInParent<DocumentClass> ().gameObject.transform;
		GameObject hero5 = (GameObject) Instantiate(heroPrefab, transform.position, transform.rotation);
		hero5.transform.parent = GetComponentInParent<DocumentClass> ().gameObject.transform;

		GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("Welcome to level " + levelNum);
	}

	public double screenTime = 0;

	public void Update() {
		if (screenTime >= ScreenTimeConst)
			return;
		screenTime += Time.deltaTime;
		if(screenTime >= ScreenTimeConst) 
			GetComponentInParent<DocumentClass> ().GetComponentInChildren<ChangeText>().chgtxt("");
	}
}
