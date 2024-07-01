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
      decimal tRef = 1762.53420222212m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = TemperatureUnit.CELSIUS.Convert(TemperatureUnit.FAHRENHEIT, valIn);
      tRef = 2254.22220222212m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.CELSIUS, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, valIn);
      tRef = 941.2532722907777777777777778m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = decimal.Round(TemperatureUnit.KELVIN.Convert(TemperatureUnit.FAHRENHEIT, conv), 10);
      Assert.That(res, Is.EqualTo(refValue));

      //Test it with impossible minus temperature
      conv = TemperatureUnit.CELSIUS.Convert(TemperatureUnit.KELVIN, -5000);
      tRef = 0m;
      Assert.That(conv, Is.EqualTo(tRef));
   }

   #endregion
}