using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


//This class is needed because wizards are created dynamically and therefore no static reference to wizards can be made in the GUI
//Wizards can be registered and the assumption is, that buttons will be destroyed after each level, so there are no dangling pointers
public class CustomEventTrigger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler{

	public enum ButtonType {
		Left,
		Right
	};
	//set this in the insepctor 
	public ButtonType buttonType = ButtonType.Left;


	private WizardController wizardController;

	
	public void OnPointerDown(PointerEventData data) {
		if (wizardController != null) {
			switch (buttonType) {
			case ButtonType.Left:
				wizardController.LeftPressed ();
				break;
			case ButtonType.Right:
				wizardController.RightPressed ();
				break;
			}
		}
	}
	
	public void OnPointerUp(PointerEventData data) {
		if (wizardController != null) {
			wizardController.Released();
		}
	}

	public void RegisterWizard(WizardController _wizardController){
		this.wizardController = _wizardController;
	}
	
}