using System;

public static class ProtocolConvert
{
    //float
    public static byte toProByte(this float f, float mutiply = 255) { return (byte)(f * mutiply);}
    public static short toProShort(this float f, float mutiply = 100) { return (short)(f * mutiply);}
    public static ushort toProUShort(this float f, float mutiply = 100) { return (ushort)(f * mutiply);}
    public static int toProInt(this float f, float mutiply = 100) { return (int)(f * mutiply);}
    public static uint toProUInt(this float f, float mutiply = 100) { return (uint)(f * mutiply);}

    public static float toProFloat(this byte i, float mutiply = 255) { return (float)i / mutiply;}
    public static float toProFloat(this short i, float mutiply = 100) { return (float)i / mutiply;}
    public static float toProFloat(this ushort i, float mutiply = 100) { return (float)i / mutiply;}
    public static float toProFloat(this int i, float mutiply = 100) { return (float)i / mutiply;}
    public static float toProFloat(this uint i, float mutiply = 100) { return (float)i / mutiply;}

    //int
    public static byte toProByte(this int i) { return (byte)i;}
    public static short toProShort(this int i) { return (short)i;}

    public static int toProInt(this byte i) { return (int)i;}
    public static int toProInt(this short i) { return (int)i;}
}

