using System.Numerics;

namespace SimpleBitPacker {
    public class BitWriter {
        private List<byte> buffer = [0];
        private int bitsInBuffer = 0;

        public void Write<T>(T value, int bits) where T : IBinaryInteger<T> {
            for (int i = 0; i < bits; i++) {
                if (bitsInBuffer / 8 > buffer.Count - 1)
                    buffer.Add(0);

                if ((value & (T.One << i)) != T.Zero)
                    buffer[buffer.Count - 1] |= (byte)(1 << bitsInBuffer % 8);

                bitsInBuffer++;
            }
        }

        public void Write(bool value) => Write(1, 1);

        public byte[] GetBytes() => buffer.ToArray();
    }
}
