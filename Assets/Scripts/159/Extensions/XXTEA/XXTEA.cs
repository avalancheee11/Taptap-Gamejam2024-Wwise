using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// XXTEA 加解密
/// </summary>
namespace GameLib
{
	/// <summary>
	/// XXTE.
	/// </summary>
	public static class XXTEA
	{
		/// <summary>
		/// Encrypt the specified Data and Key.
		/// </summary>
		/// <param name="Data">Data.</param>
		/// <param name="Key">Key.</param>
		private static byte[] Encrypt(byte[] Data, byte[] Key)
		{
			if (Data.Length == 0) {
				return Data;
			}
			return ToByteArray(Encrypt(ToUInt32Array(Data, true), ToUInt32Array(Key, false)), false);
		}

		/// <summary>
		/// Decrypt the specified Data and Key.
		/// </summary>
		/// <param name="Data">Data.</param>
		/// <param name="Key">Key.</param>
		private static byte[] Decrypt(byte[] Data, byte[] Key)
		{
			if (Data.Length == 0) {
				return Data;
			}
			return ToByteArray(Decrypt(ToUInt32Array(Data, false), ToUInt32Array(Key, false)), true);
		}

		/// <summary>
		/// Encrypt the specified v and k.
		/// </summary>
		/// <param name="v">V.</param>
		/// <param name="k">K.</param>
		private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
		{
			Int32 n = v.Length - 1;
			if (n < 1) {
				return v;
			}
			if (k.Length < 4) {
				UInt32[] Key = new UInt32[4];
				k.CopyTo(Key, 0);
				k = Key;
			}
			UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
			Int32 p, q = 6 + 52 / (n + 1);
			while (q-- > 0) {
				sum = unchecked(sum + delta);
				e = sum >> 2 & 3;
				for (p = 0; p < n; p++)
				{
					y = v[p + 1];
					z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
				}
				y = v[0];
				z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
			}
			return v;
		}

		/// <summary>
		/// Decrypt the specified v and k.
		/// </summary>
		/// <param name="v">V.</param>
		/// <param name="k">K.</param>
		private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
		{
			Int32 n = v.Length - 1;
			if (n < 1) {
				return v;
			}
			if (k.Length < 4) {
				UInt32[] Key = new UInt32[4];
				k.CopyTo(Key, 0);
				k = Key;
			}
			UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
			Int32 p, q = 6 + 52 / (n + 1);
			sum = unchecked((UInt32)(q * delta));
			while (sum != 0) {
				e = sum >> 2 & 3;
				for (p = n; p > 0; p--)
				{
					z = v[p - 1];
					y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
				}
				z = v[n];
				y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
				sum = unchecked(sum - delta);
			}
			return v;
		}

		/// <summary>
		/// Tos the U int32 array.
		/// </summary>
		/// <returns>The U int32 array.</returns>
		/// <param name="Data">Data.</param>
		/// <param name="IncludeLength">If set to <c>true</c> include length.</param>
		private static UInt32[] ToUInt32Array(byte[] Data, Boolean IncludeLength)
		{
			Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
			UInt32[] Result;
			if (IncludeLength) {
				Result = new UInt32[n + 1];
				Result[n] = (UInt32)Data.Length;
			}
			else {
				Result = new UInt32[n];
			}
			n = Data.Length;
			for (Int32 i = 0; i < n; i++) {
				Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
			}
			return Result;
		}

		/// <summary>
		/// Tos the byte array.
		/// </summary>
		/// <returns>The byte array.</returns>
		/// <param name="Data">Data.</param>
		/// <param name="IncludeLength">If set to <c>true</c> include length.</param>
		private static byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
		{
			Int32 n;
			if (IncludeLength) {
				n = (Int32)Data[Data.Length - 1];
			}
			else {
				n = Data.Length << 2;
			}

			byte[] Result = new byte[n];
			for (Int32 i = 0; i < n; i++) {
				Result[i] = (byte)(Data[i >> 2] >> ((i & 3) << 3));
			}
			return Result;
		}
			
		/// <summary>
		/// Encrypt the specified Target and Key.
		/// </summary>
		/// <param name="Target">Target.</param>
		/// <param name="Key">Key.</param>
		public static string Encrypt(string Target, string Key)
		{
			System.Text.Encoding encoder = System.Text.Encoding.UTF8;
			byte[] data = Encrypt(encoder.GetBytes(Target), encoder.GetBytes(Key));
			return System.Convert.ToBase64String(data);

		}

		/// <summary>
		/// Decrypt the specified Target and Key.
		/// </summary>
		/// <param name="Target">Target.</param>
		/// <param name="Key">Key.</param>
		public static string Decrypt(string Target, string Key)
		{
			System.Text.Encoding encoder = System.Text.Encoding.UTF8;			
			return encoder.GetString(Decrypt(System.Convert.FromBase64String(Target), encoder.GetBytes(Key)));
		}
	}
}