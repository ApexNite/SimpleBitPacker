using System.Numerics;

namespace SimpleBitPacker;

public static class BitPackingExtensions
{
    public static void WriteRange<T>(this BitWriter writer, T value, T min, T max) where T : IBinaryInteger<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(max, min);
        ArgumentOutOfRangeException.ThrowIfEqual(max, min);
        ArgumentOutOfRangeException.ThrowIfLessThan(value, min);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, max);

        if (UsesNative64<T>())
        {
            ulong nativeMin = ulong.CreateTruncating(min);
            ulong nativeSpan = unchecked(ulong.CreateTruncating(max) - nativeMin);
            ulong nativeOffset = unchecked(ulong.CreateTruncating(value) - nativeMin);
            writer.Write(nativeOffset, 64 - BitOperations.LeadingZeroCount(nativeSpan));
            return;
        }

        BigInteger bigMin = BigInteger.CreateChecked(min);
        BigInteger span = BigInteger.CreateChecked(max) - bigMin;
        int bits = checked((int)span.GetBitLength());

        BigInteger offset = BigInteger.CreateChecked(value) - bigMin;
        writer.Write(offset, bits);
    }

    public static T ReadRange<T>(this BitReader reader, T min, T max) where T : IBinaryInteger<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(max, min);
        ArgumentOutOfRangeException.ThrowIfEqual(max, min);

        if (UsesNative64<T>())
        {
            ulong nativeMin = ulong.CreateTruncating(min);
            ulong nativeSpan = unchecked(ulong.CreateTruncating(max) - nativeMin);
            ulong nativeOffset = reader.Read<ulong>(64 - BitOperations.LeadingZeroCount(nativeSpan));
            if (nativeOffset > nativeSpan)
                throw new InvalidDataException();

            return T.CreateTruncating(unchecked(nativeMin + nativeOffset));
        }

        BigInteger bigMin = BigInteger.CreateChecked(min);
        BigInteger span = BigInteger.CreateChecked(max) - bigMin;
        int bits = checked((int)span.GetBitLength());

        BigInteger offset = reader.Read<BigInteger>(bits);

        if (offset.Sign < 0)
            offset += BigInteger.One << bits;

        if (offset > span)
            throw new InvalidDataException();

        return T.CreateChecked(bigMin + offset);
    }

    private static bool UsesNative64<T>() =>
        typeof(T) == typeof(sbyte) || typeof(T) == typeof(byte) ||
        typeof(T) == typeof(short) || typeof(T) == typeof(ushort) ||
        typeof(T) == typeof(int) || typeof(T) == typeof(uint) ||
        typeof(T) == typeof(long) || typeof(T) == typeof(ulong) ||
        typeof(T) == typeof(nint) || typeof(T) == typeof(nuint);
}
