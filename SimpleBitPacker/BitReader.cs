using System.Numerics;

namespace SimpleBitPacker {
    public class BitReader(byte[] bytes) {
        private int _offset = 0;

        public T Read<T>(int bits) where T : IBinaryInteger<T> {
            T value = T.Zero;

            for (int i = 0; i < bits; i++)
                if ((bytes[_offset / 8] & (1 << _offset++ % 8)) != 0)
                    value |= T.One << i;

            return value;
        }

        public bool Read() => Read<int>(1) != 0;
    }
}