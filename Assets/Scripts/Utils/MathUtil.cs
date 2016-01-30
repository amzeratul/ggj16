using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MathUtil {
    public static float Smooth(float t) {
        return 1.0f - (Mathf.Cos(t * Mathf.PI) * 0.5f + 0.5f);
    }

    public static float Overshoot(float t) {
        return 2 * t * (1.5f - t);
    }


    // These lerps are not bound from 0 to 1, like Unity's are:

    public static float Lerp(float a, float b, float t) {
        return a + (b - a) * t;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) {
        return new Vector2(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t));
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) {
        return new Vector3(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t), Lerp(a.z, b.z, t));
    }
}
