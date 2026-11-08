using System.Numerics;

namespace SimpleBitPacker;

internal static class IntegerBitWidth<T> where T : IBinaryInteger<T>
{
    internal static readonly int Value = typeof(T) == typeof(BigInteger)
        ? int.MaxValue
        : checked(T.Zero.GetByteCount() * 8);
}
