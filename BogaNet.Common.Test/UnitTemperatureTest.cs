using BogaNet.Unit;

namespace BogaNet.Test;

public class UnitTemperatureTest
{
   #region Tests

   [Test]
   public void UnitTemperature_Convert_Test()
   {
      ExtensionUnitTemperature.IgnoreSameUnit = false;

      const decimal val = 100m;
      decimal conv;

      //Console.WriteLine(UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.KELVIN, val));
      conv = UnitTemperature.KELVIN.Convert(UnitTemperature.FAHRENHEIT, val);
      Assert.That(val, Is.EqualTo(UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.KELVIN, conv)));

      conv = UnitTemperature.CELSIUS.Convert(UnitTemperature.FAHRENHEIT, val);
      Assert.That(val, Is.EqualTo(UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.CELSIUS, conv)));

      conv = UnitTemperature.FAHRENHEIT.Convert(UnitTemperature.KELVIN, val);
      Assert.That(val, Is.EqualTo(UnitTemperature.KELVIN.Convert(UnitTemperature.FAHRENHEIT, conv)));
   }

   #endregion
}