using System;
using System.Collections.Generic;
using GameLib;

public class NumberObject
{
    public static string XXTEAKey = "z";
    public const int TypeNone = 0;
    public const int TypeCal = 1;
    public const int TypeXXTea = 2;

    public double doubleValue { get; private set; }
    public float floatValue { get; private set; }
    public int intValue { get; private set; }

    public double da { get; private set; }
    public float fa { get; private set; }
    public int ia { get; private set; }

    public string stringValue { get; private set; }
    public int type { get; private set; }

    public int a => 50 + 50.toCCRandomIndex();

    public void initialize(int type = NumberObject.TypeCal)
    {
        this.da = this.a;
        this.fa = this.a;
        this.ia = this.a;

        this.type = type;
        this.setDouble(0);
        this.setFloat(0);
        this.setInt(0);
    }

    public double getDouble()
    {
        if (this.type == TypeCal) {
            return this.doubleValue / this.da;
        }
        else if (this.type == TypeXXTea) {
            return XXTEA.Decrypt(this.stringValue, NumberObject.XXTEAKey).toDouble();
        }
        else {
            return this.doubleValue;
        }
    }

    public void setDouble(double val)
    {
        if (this.type == TypeCal) {
            this.doubleValue = this.da * val;
        }
        else if (this.type == TypeXXTea) {
            this.stringValue = XXTEA.Encrypt(val.ToString(), NumberObject.XXTEAKey);
        }
        else {
            this.doubleValue = val;
        }
    }

    public float getFloat()
    {
        if (this.type == TypeCal) {
            return this.floatValue / this.fa;
        }
        else if (this.type == TypeXXTea) {
            return XXTEA.Decrypt(this.stringValue, NumberObject.XXTEAKey).toFloat();
        }
        else {
            return this.floatValue;
        }
    }

    public void setFloat(float val)
    {
        if (this.type == TypeCal) {
            this.floatValue = this.fa * val;
        }
        else if (this.type == TypeXXTea) {
            this.stringValue = XXTEA.Encrypt(val.ToString(), NumberObject.XXTEAKey);
        }
        else {
            this.floatValue = val;
        }
    }

    public int getInt()
    {
        if (this.type == TypeCal) {
            return this.intValue / this.ia;
        }
        else if (this.type == TypeXXTea) {
            return XXTEA.Decrypt(this.stringValue, NumberObject.XXTEAKey).toInt();
        }
        else {
            return this.intValue;
        }
    }

    public void setInt(int val)
    {
        if (this.type == TypeCal) {
            this.intValue = this.ia * val;
        }
        else if (this.type == TypeXXTea) {
            this.stringValue = XXTEA.Encrypt(val.ToString(), NumberObject.XXTEAKey);
        }
        else {
            this.intValue = val;
        }
    }
}