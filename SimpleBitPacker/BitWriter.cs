using System.Numerics;

namespace SimpleBitPacker
{
    public class BitWriter
    {
        private readonly List<byte> _buffer = [0];
        private int _bitsInBuffer = 0;

        public void Write<T>(T value, int bits) where T : IBinaryInteger<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegative(bits, nameof(bits));
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

        public void Write(bool value) => Write(value ? 1 : 0, 1);

        public byte[] GetBytes() => [.. _buffer];
    }
}
