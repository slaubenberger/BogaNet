using BogaNet.Unit;
using BogaNet.Extension;

namespace BogaNet.Test.Unit;

public class TemperatureUnitTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      TemperatureUnitExtension.IgnoreSameUnit = false;
   }

   #region Tests

   [Test]
   public void UnitTemperature_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, valIn);
      decimal res = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = TemperatureUnit.CELSIUS.Convert(TemperatureUnit.FAHRENHEIT, valIn);
      res = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.CELSIUS, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, valIn);
      res = Decimal.Round(TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, conv), 10);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}