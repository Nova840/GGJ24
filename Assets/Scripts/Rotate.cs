using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private Vector3 rotateSpeed;

    [SerializeField]
    private Space space;

    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime, space);
    }

}