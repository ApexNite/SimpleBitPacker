using System.Numerics;

namespace SimpleBitPacker {
    public class BitReader {
        private byte[] bytes;
        private int offset = 0;

        public BitReader(byte[] bytes) {
            this.bytes = bytes;
        }

        public T Read<T>(int bits) where T : IBinaryInteger<T> {
            T value = T.Zero;

            for (int i = 0; i < bits; i++)
                value |= (T)Convert.ChangeType(((bytes[offset / 8] & (1 << offset++ % 8)) != 0 ? 1UL : 0UL) << i, typeof(T));

            return value;
        }

        public bool ReadBool() => (bytes[offset / 8] & (1 << offset++ % 8)) != 0;
    }
}