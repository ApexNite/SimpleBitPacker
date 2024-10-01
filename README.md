# Simple Bit Packer
A simple ~60 line C# implementation of bit packing compression.

## What is Bit Packing?
Bit packing is widely used to compress data that follows a size range different than that of what bytes, shorts, ints, and longs
usually allow by letting you explicitly state the number of bits needed to represent a range of integers.

For example, if you need to transport an array of integers that will never be negative and will never exceed 10,000,000, you only need
24 bits. Usually, this would require using multiple uints of 32 bits, where 8 bits will be completely wasted. But with bit packing,
you can specify only to use 24 of the 32 bits, reducing packet size by ~0.25%.

## Writing Numbers
The `BitWriter` class contains two methods, `Write<T>(T value, int bits)` and `Write(bool value, int bits)`, that write
individual bits from an input to create a new compressed byte array. Use `GetBytes()` to get compressed bytes.
```csharp
// Create a new BitWriter object
BitWriter bitWriter = new BitWriter();

// Write integers
bitWriter.Write<byte>(13, 4);
bitWriter.Write<sbyte>(-6, 4);
bitWriter.Write<short>(-3159, 13);
bitWriter.Write<ushort>(6202, 13);
bitWriter.Write<int>(165041, 19);
bitWriter.Write<uint>(499167, 19);
bitWriter.Write<long>(8917127262193580, 54);
bitWriter.Write<ulong>(17924326516934600, 54);

// Write bool
bitWriter.Write(true);

// Get byte array
byte[] compressedBytes = bitWriter.GetBytes();
```

## Reading Numbers
The `BitReader` class contains two methods, `Read<T>(int bits)` and `ReadBool(int bits)`, that read
individual bits from a compressed byte array and return their uncompressed value.

```csharp
// Create a new BitReader object
BitReader bitReader = new BitReader(compressedBytes);

// Read integers
byte byteValue = bitReader.Read<byte>(4)
sbyte sbyteValue = bitReader.Read<sbyte>(4);
short shortValue = bitReader.Read<short>(13);
ushort ushortValue = bitReader.Read<ushort>(13);
int intValue = bitReader.Read<int>(19);
uint uintValue = bitReader.Read<uint>(19);
long longValue = bitReader.Read<long>(54);
ulong ulongValue = bitReader.Read<ulong>(54);

// Read bool
boolValue = bitReader.ReadBool();
```

### Note:
This project was made with minimal supplementary performance optimizations and a unique use case (BigInteger and IntPtr mostly)
testing. This was a fun side project that fulfilled the requirements I needed. If you plan to use this in your project, be warned that
it was **never intended for use in commercial applications** and should be used as a basis to build off of.
