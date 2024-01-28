using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMove : MonoBehaviour {

    [SerializeField]
    private float noiseScale = .1f;

    [SerializeField]
    private float noiseStrength = .25f;

    [SerializeField]
    private float noiseScaleRotate = .1f;

    [SerializeField]
    private float noiseStrengthRotate = 10;

    private Vector3 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    private void Update() {
        float x = (Mathf.PerlinNoise(0, Time.time * noiseScale) * 2 - 1) * noiseStrength;
        float y = (Mathf.PerlinNoise(Time.time * noiseScale, 0) * 2 - 1) * noiseStrength;
        float r = (Mathf.PerlinNoise(0, Time.time * noiseScaleRotate + 128) * 2 - 1) * noiseStrengthRotate;

        transform.position = initialPosition + new Vector3(x, y, 0);
        transform.localEulerAngles = new Vector3(0, 180, r);
    }

}