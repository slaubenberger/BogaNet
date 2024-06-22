using BogaNet.Unit;

namespace BogaNet.Test;

public class TemperatureUnitTest
{
   #region Tests

   [Test]
   public void UnitTemperature_Convert_Test()
   {
      TemperatureUnitExtension.IgnoreSameUnit = false;

      const decimal val = 100m;
      decimal conv;

      //Console.WriteLine(UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.KELVIN, val));
      conv = TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, val);
      Assert.That(val, Is.EqualTo(TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, conv)));

      conv = TemperatureUnit.CELSIUS.Convert(TemperatureUnit.FAHRENHEIT, val);
      Assert.That(val, Is.EqualTo(TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.CELSIUS, conv)));

      conv = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, val);
      Assert.That(val, Is.EqualTo(TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, conv)));
   }

   #endregion
}