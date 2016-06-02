using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShieldController : NetworkBehaviour {
	[HideInInspector]
	public GameObject wizard;
	private Vector3 offset;

	[SyncVar]
	public NetworkInstanceId wizardNetId;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (-0.05f, -0.28f, 0.0f);
	}

	public override void OnStartClient()
	{
		// When we are spawned on the client,
		// find the parent object using its ID,
		// and set it to be our transform's parent.
		wizard = ClientScene.FindLocalObject(wizardNetId);
	}
	
	// Update is called once per frame
	void Update () {
		if (wizard == null) {
			Debug.LogError ("Wizard in shieldcontroller is null");
			return;
		}

		//hack to follow wizard, because setting shield to chiild of wizard makes shield invisble (a unity bug...)
		this.transform.position = wizard.transform.position + offset;
	}
}
