using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static bool IsInMask(this LayerMask layerMask, int layer) {
        return layerMask == (layerMask | (1 << layer));
    }

}