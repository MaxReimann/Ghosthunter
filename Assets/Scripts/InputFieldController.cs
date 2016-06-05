using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InputFieldController : MonoBehaviour {

	private InputField input;
	private GameManager manager;

	// Use this for initialization
	void Start () {
		input = GetComponent<InputField> ();
		manager = GameManager.GetInstance ();
	}

	public void setPlayerName(){
		manager.setPlayerName (input.text);
	}

	public void setNetworkAdress(){
		NetworkManager.singleton.networkAddress = input.text;
	}
}
