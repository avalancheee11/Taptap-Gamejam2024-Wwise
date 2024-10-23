using System;
using UnityEngine;

public static class Vector2Convert
{
    public static bool similar(Vector2 vecA, Vector2 vecB)
    {
        return similar(vecA, vecB, 10);
    }

    public static bool similar(Vector2 vecA, Vector2 vecB, float offset)
    {
        return similar(vecA, vecB, offset.toInt());
    }

    public static bool similar(Vector2 vecA, Vector2 vecB, int offset)
    {
        if (vecA == Vector2.zero || vecB == Vector2.zero) return false;
        int x = (int)(vecA.x - vecB.x);
        int y = (int)(vecA.y - vecB.y);
        return Mathf.Abs(x) < offset && Mathf.Abs(y) < offset;
    }

    public static float radian(Vector2 fromValue, Vector2 toValue, int digits = 0)
    {
        return radian(toValue - fromValue, digits);
    }

    public static float radian(this Vector2 value, int digits = 0)
    {
        float r = Mathf.Atan2(value.y, value.x);
        if (digits > 0) {
            r = (float)Math.Round((double)r, digits);
        }
        return r;
    }

    //已知 顶点c, 点p, 及方向u,求得u和p-c形成的夹角
    public static float radian(Vector2 cVec, float ux, float uy, Vector2 pVec)
    {
        float cx = cVec.x;
        float cy = cVec.y;
        float px = pVec.x;
        float py = pVec.y;
        // D = P - C
        float dx = px - cx;
        float dy = py - cy;

        // |D| = (dx^2 + dy^2)^0.5
        float length = Mathf.Sqrt(dx * dx + dy * dy);

        if (length.isZero()) {
            return 0;
        }

        // Normalize D
        dx /= length;
        dy /= length;

        // acos(D dot U)
        return Mathf.Acos(Mathf.Clamp(dx * ux + dy * uy, -1, 1));
    }

    public static float angle(Vector2 fromValue, Vector2 toValue)
    {
        return radian(fromValue, toValue).toAngle();
    }

    public static float angle(this Vector2 value)
    {
        return radian(value).toAngle();
    }

    public static float angle(Vector2 cVec, float ux, float uy, Vector2 pVec)
    {
        return radian(cVec, ux, uy, pVec).toAngle();
    }

    public static ParabolaData getParabolaConfig(Vector2 vecA, Vector2 vecB, Vector2 vecC)
    {
        return getParabolaConfig(vecA, vecB, vecC, true);
    }

    public static ParabolaData getParabolaConfig(Vector2 vecA, Vector2 vecB, Vector2 vecC, bool vertical)
    {
        float x1 = vecA.x;
        float y1 = vecA.y;
        float x2 = vecB.x;
        float y2 = vecB.y;
        float x3 = vecC.x;
        float y3 = vecC.y;

        float a = 0, b = 0, c = 0;
        if (vertical) {
            a = ((y2 - y1) * (x3 - x1) - (y3 - y1) * (x2 - x1)) / ((x2 * x2 - x1 * x1) * (x3 - x1) - (x3 * x3 - x1 * x1) * (x2 - x1));
            b = ((y2 - y1) - (x2 * x2 - x1 * x1) * a) / (x2 - x1);
            c = y1 - x1 * x1 * a - x1 * b;
        }
        else {
            a = ((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1)) / ((y2 * y2 - y1 * y1) * (y3 - y1) - (y3 * y3 - y1 * y1) * (y2 - y1));
            b = ((x2 - x1) - (y2 * y2 - y1 * y1) * a) / (y2 - y1);
            c = x1 - y1 * y1 * a - y1 * b;
        }

        return new ParabolaData(a, b, c);
    }
}

