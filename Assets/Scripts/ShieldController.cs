using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

	private GameObject wizard;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		wizard = GameObject.FindGameObjectWithTag ("Wizards"); 
		offset = new Vector3 (-0.05f, -0.28f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//hack to follow wizard, because setting shield to chiild of wizard makes shield invisble (a unity bug...)
		this.transform.position = wizard.transform.position + offset;
	}
}
