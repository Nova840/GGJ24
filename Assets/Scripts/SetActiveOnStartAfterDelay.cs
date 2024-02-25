using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnStartAfterDelay : MonoBehaviour {

    [SerializeField]
    private float delay;

    [SerializeField]
    private GameObject[] setActive;

    private IEnumerator Start() {
        foreach (GameObject g in setActive) {
            g.SetActive(false);
        }
        yield return new WaitForSeconds(delay);
        foreach (GameObject g in setActive) {
            g.SetActive(true);
        }
    }

}