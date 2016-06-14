using UnityEngine;
using System.Collections;

public interface IChildDelegator
{
	// interface members
	void childEnterCollision2D (Collision2D coll);
	void childEnterTrigger2D (Collider2D coll);
}

//use this class to delegate events of children to parents
public class ChildDelegate : MonoBehaviour {

	[Tooltip("This controller must implement interface IChildDelegator!")]
	public MonoBehaviour parentController;

	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		//will through errors, if parentcontroller doesnt implement methods.
		(parentController as IChildDelegator).childEnterCollision2D (coll);
	}
	
	void OnEnterTrigger2D(Collider2D coll){
		(parentController as IChildDelegator).childEnterTrigger2D (coll);
	}
	
	
	
}
