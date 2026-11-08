using System.Numerics;

namespace SimpleBitPacker
{
    /// <summary>Reads packed integers and booleans from a byte array.</summary>
    /// <param name="bytes">The packed data to read.</param>
    /// <exception cref="ArgumentNullException">The byte array is null.</exception>
    public class BitReader(byte[] bytes)
    {
        private readonly byte[] _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        private int _offset = 0;

        /// <summary>Reads the next integer.</summary>
        /// <param name="bits">A positive bit count. For fixed-width types, use no more than the type's width.</param>
        /// <exception cref="ArgumentOutOfRangeException">The bit count is not positive, exceeds the type's width, or would overflow the total bit offset.</exception>
        /// <exception cref="EndOfStreamException">Not enough bits remain.</exception>
        public T Read<T>(int bits) where T : IBinaryInteger<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bits, nameof(bits));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bits, IntegerBitWidth<T>.Value, nameof(bits));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bits, int.MaxValue - _offset, nameof(bits));
            if (bits > (long)_bytes.Length * 8 - _offset)
                throw new EndOfStreamException();

            if (bits == 1)
            {
                bool set = (_bytes[_offset / 8] & (1 << (_offset++ % 8))) != 0;

                if (!set)
                    return T.Zero;

                return T.IsNegative(T.CreateTruncating(-1)) ? T.CreateTruncating(-1) : T.One;
            }

            T value = T.Zero;
            int read = 0;
            while (read < bits)
            {
                int shift = _offset % 8;
                int take = Math.Min(8 - shift, bits - read);
                int chunk = (_bytes[_offset / 8] >> shift) & ((1 << take) - 1);

                value |= T.CreateTruncating(chunk) << read;
                _offset += take;
                read += take;
            }

            if (bits > 0 && T.IsNegative(T.CreateTruncating(-1)) && !T.IsNegative(value) &&
                (value & (T.One << (bits - 1))) != T.Zero)
                value |= T.CreateTruncating(-1) << bits;

            return value;
        }

        /// <summary>Reads the next bit as a boolean.</summary>
        /// <exception cref="EndOfStreamException">No bits remain.</exception>
        public bool Read() => Read<int>(1) != 0;

        /// <summary>Returns the next integer without advancing the reader.</summary>
        /// <param name="bits">A positive bit count. For fixed-width types, use no more than the type's width.</param>
        /// <exception cref="ArgumentOutOfRangeException">The bit count is not positive, exceeds the type's width, or would overflow the total bit offset.</exception>
        /// <exception cref="EndOfStreamException">Not enough bits remain.</exception>
        public T Peek<T>(int bits) where T : IBinaryInteger<T>
        {
            int offset = _offset;
            try
            {
                return Read<T>(bits);
            }
            finally
            {
                _offset = offset;
            }
        }

        /// <summary>Returns the next boolean without advancing the reader.</summary>
        /// <exception cref="EndOfStreamException">No bits remain.</exception>
        public bool Peek() => Peek<int>(1) != 0;
    }
}