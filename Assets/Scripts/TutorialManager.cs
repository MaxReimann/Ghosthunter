using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager {

	private static TutorialManager instance=null;
	
	private TutorialManager(){
	}
	
	public static TutorialManager Instance{
		get{
			if (instance==null){
				instance = new TutorialManager();
			}
			return instance;
		}
	}

	public bool wasLeft;
	public bool wasRight;
	public bool hasShot;
	public bool hasShotGhost;
}
