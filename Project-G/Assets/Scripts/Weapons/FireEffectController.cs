using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectController : MonoBehaviour {

    void Start() {
      StartCoroutine(TurnOffLight());
    }

    IEnumerator TurnOffLight() {
		  yield return new WaitForSeconds(0);
		  gameObject.SetActive(false);
    }
}
