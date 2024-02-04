using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnStartAfterDelay : MonoBehaviour {

    [SerializeField]
    private bool active;

    [SerializeField]
    private float delay;

    [SerializeField]
    private GameObject[] setActive;

    private IEnumerator Start() {
        yield return new WaitForSeconds(delay);
        foreach (GameObject g in setActive) {
            g.SetActive(active);
        }
    }

}