using UnityEngine;

public class CursorController : MonoBehaviour {

    void Start() {
        Cursor.visible = false;
    }
    
    void Update() {
        transform.position = Input.mousePosition;
    }
}
