using System.Numerics;

namespace SimpleBitPacker {
    public class BitWriter {
        private readonly List<byte> _buffer = [0];
        private int _bitsInBuffer = 0;

        public void Write<T>(T value, int bits) where T : IBinaryInteger<T> {
            for (int i = 0; i < bits; i++) {
                if (_bitsInBuffer / 8 > _buffer.Count - 1)
                    _buffer.Add(0);

                if ((value & (T.One << i)) != T.Zero)
                    _buffer[^1] |= (byte)(1 << _bitsInBuffer % 8);

                _bitsInBuffer++;
            }
        }

        public void Write(bool value) => Write(value ? 1 : 0, 1);

        public byte[] GetBytes() => _buffer.ToArray();
    }
}
