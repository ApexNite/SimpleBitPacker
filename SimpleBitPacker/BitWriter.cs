using System.Numerics;

namespace SimpleBitPacker
{
    /// <summary>Packs integers and booleans into a byte array.</summary>
    public class BitWriter
    {
        private readonly List<byte> _buffer = [0];
        private int _bitsInBuffer = 0;

        /// <summary>Writes the lowest requested bits of an integer.</summary>
        /// <param name="value">The integer to write.</param>
        /// <param name="bits">A positive bit count. For fixed-width types, use no more than the type's width.</param>
        /// <exception cref="ArgumentOutOfRangeException">The bit count is not positive, exceeds the type's width, or would overflow the total bit offset.</exception>
        public void Write<T>(T value, int bits) where T : IBinaryInteger<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bits, nameof(bits));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bits, IntegerBitWidth<T>.Value, nameof(bits));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bits, int.MaxValue - _bitsInBuffer, nameof(bits));

            int written = 0;
            while (written < bits)
            {
                int shift = _bitsInBuffer % 8;
                int take = Math.Min(8 - shift, bits - written);
                int chunk = byte.CreateTruncating(value >> written) & ((1 << take) - 1);

                if (_bitsInBuffer / 8 == _buffer.Count)
                    _buffer.Add(0);

                _buffer[^1] |= (byte)(chunk << shift);
                _bitsInBuffer += take;
                written += take;
            }
        }

        /// <summary>Writes a boolean as one bit.</summary>
        public void Write(bool value) => Write(value ? 1 : 0, 1);

        /// <summary>Returns a copy of the packed data.</summary>
        public byte[] GetBytes() => [.. _buffer];
    }
}
