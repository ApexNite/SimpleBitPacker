using System.Numerics;

namespace SimpleBitPacker {
    public class BitReader(byte[] bytes) {
        private int _offset = 0;

        public T Read<T>(int bits) where T : IBinaryInteger<T> {
            T value = T.Zero;

            for (int i = 0; i < bits; i++)
                value |= (T)Convert.ChangeType(((bytes[_offset / 8] & (1 << _offset++ % 8)) != 0 ? 1UL : 0UL) << i, typeof(T));

            return value;
        }

        public bool Read() => Read<int>(1) != 0;
    }
}