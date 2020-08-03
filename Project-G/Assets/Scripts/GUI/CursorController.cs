using UnityEngine;

public class CursorController : MonoBehaviour {

    void Start() {
        ToggleShowMouse(false);
    }
    
    void Update() {
        transform.position = Input.mousePosition;
    }

    public void ToggleShowMouse(bool show) {
        Cursor.visible = show;
    }


}
