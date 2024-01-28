using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField]
    private float time = 30;

    private void Start() {
        Destroy(gameObject, time);
    }

}