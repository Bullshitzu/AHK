using UnityEngine;
using System.Collections;

public static class Utility {

    public static Rect AddRects (Rect a, Rect b) {
        return new Rect(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height);
    }

    public static float QuaternionMagnitude (Quaternion input) {
        return Mathf.Sqrt(input.x * input.x + input.y * input.y + input.z * input.z + input.w * input.w);
    }

}
