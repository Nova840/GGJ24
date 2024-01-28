using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadSound : MonoBehaviour {

    private void Start() {
        Sound.LoadSoundPrefabIfNone();
    }

}