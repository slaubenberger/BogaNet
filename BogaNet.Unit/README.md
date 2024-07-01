# BogaNet.Unit
Various units, like area, bit, byte, length, temperature, volume and weight with easy conversion between different types.

## Units and example code
```csharp
decimal yard2 = AreaUnit.M2.Convert(AreaUnit.YARD2, 12); //Meter² to yards²
decimal kbit = BitUnit.BIT.Convert(BitUnit.kbit, 1200); //Bit to kbit
decimal kB = ByteUnit.BYTE.Convert(ByteUnit.kB, 1976); //Byte to kB
decimal meter = LengthUnit.YARD.Convert(LengthUnit.M, 9); //Yard to meter
decimal kelvin = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, 7800); //Fahrenheit to Kelvin
decimal pint = VolumeUnit.LITER.Convert(VolumeUnit.PINT_US, 5); //Liter to pint
decimal pound = WeightUnit.GRAM.Convert(WeightUnit.POUND, 150); //Gram to pound
```

## Nuget:
[BogaNet.Unit](https://www.nuget.org/packages/BogaNet.Unit/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)
