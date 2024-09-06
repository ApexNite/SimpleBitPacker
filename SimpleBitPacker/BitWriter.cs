using System.Numerics;

namespace SimpleBitPacker {
    public class BitWriter {
        private List<byte> buffer = [0];
        private int bitsInBuffer = 0;

        public void Write<T>(T value, int bits) where T : IBinaryInteger<T> {
            for (int i = 0; i < bits; i++)
                WriteBit((value & (T.One << i)) != T.Zero);
        }

        public void Write(float value, int bits) => Write(BitConverter.ToInt32(BitConverter.GetBytes(value)), bits);

        public void Write(bool value) => WriteBit(value);

        public byte[] GetBytes() => buffer.ToArray();

        private void WriteBit(bool bit) {
            if (bitsInBuffer / 8 > buffer.Count - 1)
                buffer.Add(0);

            if (bit)
                buffer[buffer.Count - 1] |= (byte)(1 << bitsInBuffer % 8);

            bitsInBuffer++;
        }
    }
}
