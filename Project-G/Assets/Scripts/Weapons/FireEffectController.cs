using System.Collections;
using UnityEngine;

public class FireEffectController : MonoBehaviour {

    private void Start() {
      StartCoroutine(TurnOffLight());
    }

    private IEnumerator TurnOffLight() {
		  yield return new WaitForSeconds(0.05f);
		  Destroy(gameObject);
    }
}
