using UnityEngine;

public class CursorController : MonoBehaviour {

	private void Start() {
		ToggleShowMouse(false);
	}
	
	private void Update() {
		transform.position = Input.mousePosition;
	}

	public void ToggleShowMouse(bool show) {
		Cursor.visible = show;
	}


}
