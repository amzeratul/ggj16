using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MathUtil {
    public static float Smooth(float t) {
        return 1.0f - (Mathf.Cos(t * Mathf.PI) * 0.5f + 0.5f);
    }
}
