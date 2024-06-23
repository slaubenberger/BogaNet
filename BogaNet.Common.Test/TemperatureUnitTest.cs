using BogaNet.Unit;

namespace BogaNet.Test;

public class TemperatureUnitTest
{
   #region Tests

   [Test]
   public void UnitTemperature_Convert_Test()
   {
      TemperatureUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1234.5678901234;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      //Console.WriteLine(UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.KELVIN, val));
      conv = TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, valIn);
      Assert.That(val, Is.EqualTo(TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, conv)));

      conv = TemperatureUnit.CELSIUS.Convert(TemperatureUnit.FAHRENHEIT, valIn);
      Assert.That(val, Is.EqualTo(TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.CELSIUS, conv)));

      conv = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, valIn);
      Assert.That(val, Is.EqualTo(Decimal.Round(TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, conv), 10)));
   }

   #endregion
}