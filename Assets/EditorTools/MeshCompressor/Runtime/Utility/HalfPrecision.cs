using System;
using UnityEngine;

public static class HalfPrecision
{
    public static byte[] FloatToHalf(float[] values)
    {
        byte[] buffer = new byte[values.Length * 2];
        for (int i = 0; i < values.Length; i++)
        {
            ushort half = FloatToUInt16(values[i]);
            buffer[i * 2] = (byte)(half & 0xFF);
            buffer[i * 2 + 1] = (byte)(half >> 8);
        }
        return buffer;
    }

    public static float[] HalfToFloat(byte[] bytes)
    {
        float[] values = new float[bytes.Length / 2];
        for (int i = 0; i < values.Length; i++)
        {
            ushort half = (ushort)((bytes[i * 2 + 1] << 8) | bytes[i * 2]);
            values[i] = UInt16ToFloat(half);
        }
        return values;
    }

    private static ushort FloatToUInt16(float value)
    {
        int i = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        int sign = (i >> 16) & 0x8000;
        int v = i & 0x7FFFFFFF;

        if (v >= 0x47800000) // 太大转为Infinity
            return (ushort)(sign | 0x7C00);
        
        if (v >= 0x38800000) // 规格化数
            return (ushort)(sign | ((v - 0x38000000) >> 13));
        
        return (ushort)sign; // 太小转为0
    }

    private static float UInt16ToFloat(ushort half)
    {
        int sign = (half >> 15) & 0x1;
        int exp = (half >> 10) & 0x1F;
        int frac = half & 0x3FF;

        if (exp == 0)
            return frac == 0 ? 0f : (sign * -2f + 1f) * (frac / 1024f) * 0.0009765625f;
        
        if (exp == 31)
            return frac == 0 ? (sign == 1 ? float.NegativeInfinity : float.PositiveInfinity) : float.NaN;

        float normalized = 1f + (frac / 1024f);
        float exponent = Mathf.Pow(2, exp - 15);
        return (sign * -2f + 1f) * normalized * exponent;
    }
}