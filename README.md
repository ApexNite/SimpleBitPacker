# Simple Bit Packer
A simple C# implementation of bit packing compression.

## Writing Numbers
The `BitWriter` class contains three methods, `Write<T>(T value, int bits)`, `Write(bool value)` and `GetBytes()`, 
that are used to write individual bits from an input to create a new compressed byte array.
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
The `BitReader` class contains two methods, `Read<T>(int bits)` and `Read()`, that read
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
boolValue = bitReader.Read();
```

### Note:
This project was made with minimal supplementary performance optimizations and unique use case (BigInteger and IntPtr mostly)
testing. This was a fun side project that has fulfilled the requirements I needed. If you plan to use this in your project, be warned
that it was **never intended for use in commercial applications** and bugfixes are not guaranteed.
