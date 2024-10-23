using System;
using UnityEngine;

public class ParabolaData
{
    public float a;
    public float b;
    public float c;

    public ParabolaData(float a, float b, float c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }
}

public static class Vector3Convert
{
    public static bool similar(Vector3 vecA, Vector3 vecB)
    {
        return similar(vecA, vecB, 10);
    }

    public static bool similar(Vector3 vecA, Vector3 vecB, float offset)
    {
        return similar(vecA, vecB, offset.toInt());
    }

    public static bool similar(Vector3 vecA, Vector3 vecB, int offset)
    {
        if(vecA == Vector3.zero || vecB == Vector3.zero) return false;
        int x = (int)(vecA.x - vecB.x);
        int y = (int)(vecA.y - vecB.y);
        return Mathf.Abs(x) < offset && Mathf.Abs(y) < offset;
    }

    public static float radian(Vector3 fromValue, Vector3 toValue, int digits = 0)
    {
        return radian(toValue - fromValue, digits);
    }

    public static float radian(this Vector3 value, int digits = 0)
    {
        float r = Mathf.Atan2(value.y, value.x);
        if (digits > 0) {
            r = (float)Math.Round((double)r, digits);
        }
        return r;
    }

    //已知 顶点c, 点p, 及方向u,求得u和p-c形成的夹角
    public static float radian(Vector3 cVec, float ux, float uy, Vector3 pVec)
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

        if(length.isZero()) {
            return 0;
        }

        // Normalize D
        dx /= length;
        dy /= length;

        // acos(D dot U)
        return Mathf.Acos(Mathf.Clamp(dx * ux + dy * uy, -1, 1));
    }

    public static float angle(Vector3 fromValue, Vector3 toValue)
    {
        return radian(fromValue, toValue).toAngle();
    }

    public static float angle(this Vector3 value)
    {
        return radian(value).toAngle();
    }

    public static float angle(Vector3 cVec, float ux, float uy, Vector3 pVec)
    {
        return radian(cVec, ux, uy, pVec).toAngle();
    }

    public static ParabolaData getParabolaConfig(Vector3 vecA, Vector3 vecB, Vector3 vecC)
    {
        return getParabolaConfig(vecA, vecB, vecC, true);
    }

    public static ParabolaData getParabolaConfig(Vector3 vecA, Vector3 vecB, Vector3 vecC, bool vertical)
    {
        float x1 = vecA.x;
        float y1 = vecA.y;
        float x2 = vecB.x;
        float y2 = vecB.y;
        float x3 = vecC.x;
        float y3 = vecC.y;

        float a = 0, b = 0, c = 0;
        if(vertical) {
            a = ((y2-y1)*(x3-x1) - (y3-y1)*(x2-x1)) /((x2*x2 - x1*x1)*(x3-x1) - (x3*x3 - x1*x1)*(x2-x1));
            b = ((y2-y1) - (x2*x2 - x1*x1) * a) / (x2-x1);
            c = y1 - x1*x1*a - x1*b;
        }
        else {
            a = ((x2-x1)*(y3-y1) - (x3-x1)*(y2-y1)) /((y2*y2 - y1*y1)*(y3-y1) - (y3*y3 - y1*y1)*(y2-y1));
            b = ((x2-x1) - (y2*y2 - y1*y1) * a) / (y2-y1);
            c = x1 - y1*y1*a - y1*b;
        }

        return new ParabolaData(a, b ,c);
    }

    public static float getParabolaSlope(float a, float b, float x)
    {
        return 2 * a * x + b;
    }

    public static Quaternion ToQ (Vector3 v)
    {
        return ToQ (v.y, v.x, v.z);
    }

    public static Quaternion ToQ (float yaw, float pitch, float roll)
    {
        yaw *= Mathf.Deg2Rad;
        pitch *= Mathf.Deg2Rad;
        roll *= Mathf.Deg2Rad;
        float rollOver2 = roll * 0.5f;
        float sinRollOver2 = (float)Math.Sin ((double)rollOver2);
        float cosRollOver2 = (float)Math.Cos ((double)rollOver2);
        float pitchOver2 = pitch * 0.5f;
        float sinPitchOver2 = (float)Math.Sin ((double)pitchOver2);
        float cosPitchOver2 = (float)Math.Cos ((double)pitchOver2);
        float yawOver2 = yaw * 0.5f;
        float sinYawOver2 = (float)Math.Sin ((double)yawOver2);
        float cosYawOver2 = (float)Math.Cos ((double)yawOver2);
        Quaternion result;
        result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
        result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
        result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
        result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

        return result;
    }

    public static Vector3 FromQ2 (this Quaternion q1)
    {
        float sqw = q1.w * q1.w;
        float sqx = q1.x * q1.x;
        float sqy = q1.y * q1.y;
        float sqz = q1.z * q1.z;
        float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        float test = q1.x * q1.w - q1.y * q1.z;
        Vector3 v;

        if (test>0.4995f*unit) { // singularity at north pole
            v.y = 2f * Mathf.Atan2 (q1.y, q1.x);
            v.x = Mathf.PI / 2;
            v.z = 0;
            return NormalizeAngles (v * Mathf.Rad2Deg);
        }
        if (test<-0.4995f*unit) { // singularity at south pole
            v.y = -2f * Mathf.Atan2 (q1.y, q1.x);
            v.x = -Mathf.PI / 2;
            v.z = 0;
            return NormalizeAngles (v * Mathf.Rad2Deg);
        }
        Quaternion q = new Quaternion (q1.w, q1.z, q1.x, q1.y);
        v.y = (float)Math.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
        v.x = (float)Math.Asin (2f * (q.x * q.z - q.w * q.y));                             // Pitch
        v.z = (float)Math.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
        return NormalizeAngles (v * Mathf.Rad2Deg);
    }

    static Vector3 NormalizeAngles (Vector3 angles)
    {
        angles.x = NormalizeAngle (angles.x);
        angles.y = NormalizeAngle (angles.y);
        angles.z = NormalizeAngle (angles.z);
        return angles;
    }

    static float NormalizeAngle (float angle)
    {
//        while (angle>360)
//            angle -= 360;
//        while (angle<0)
//            angle += 360;
//        return angle;
        return angle;
    }
}
