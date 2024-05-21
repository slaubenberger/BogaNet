namespace BogaNet;

/// <summary>
/// Units for areas
/// </summary>
public enum UnitArea
{
   M2,
   MM2,
   CM2,
   AREA,
   HECTARE,
   KM2,
   FOOT2,
   YARD2,
   PERCH,
   ACRE,
   MILE2
}

//		public static final BigDecimal FACTOR_MILE2_TO_KM2 = new BigDecimal("2.5899881103"); //square mile (terrestrial) to kilometers^2

/// <summary>
/// Extension methods for UnitArea
/// </summary>
public static class ExtensionUnitArea
{
   public const decimal FACTOR_MM2_TO_CM2 = 100; //millimeters^2 to centimeters^2
   public const decimal FACTOR_CM2_TO_M2 = 10000; //centimeters^2 to meters^2
   public const decimal FACTOR_M2_TO_AREA = 100; //meters^2 to area
   public const decimal FACTOR_AREA_TO_HECTARE = 100; //area to hectare
   public const decimal FACTOR_HECTARE_TO_KM2 = 100; //hectare to kilometers^2
   public const decimal FACTOR_FOOT2_TO_M2 = 0.09290304m; //square foot to meters^2
   public const decimal FACTOR_YARD2_TO_M2 = 0.83612736m; //square yard to meters^2
   public const decimal FACTOR_PERCH_TO_M2 = 25.2928526m; //square perch/rod to meters^2
   public const decimal FACTOR_ACRE_TO_M2 = 4046.8564224m; //acre to meters^2
   public const decimal FACTOR_MILE2_TO_KM2 = 2.5899881103m; //square mile (terrestrial) to kilometers^2

   public static decimal FACTOR_MM2_TO_M2 => FACTOR_MM2_TO_CM2 * FACTOR_CM2_TO_M2;
   public static decimal FACTOR_M2_TO_HECTARE => FACTOR_M2_TO_AREA * FACTOR_AREA_TO_HECTARE;
   public static decimal FACTOR_M2_TO_KM2 => FACTOR_M2_TO_AREA * FACTOR_AREA_TO_HECTARE * FACTOR_HECTARE_TO_KM2;

   public static decimal FACTOR_M2_TO_MILE2 => FACTOR_M2_TO_KM2 * FACTOR_MILE2_TO_KM2;

   public static decimal Convert(this UnitArea fromUnit, UnitArea toUnit, decimal inVal)
   {
      decimal val = inVal;
      decimal outVal = inVal;

      //Convert to m2
      switch (fromUnit)
      {
         case UnitArea.M2:
            val = inVal;
            break;
         case UnitArea.MM2:
            val = inVal / FACTOR_MM2_TO_M2;
            break;
         case UnitArea.CM2:
            val = inVal / FACTOR_CM2_TO_M2;
            break;
         case UnitArea.AREA:
            val = inVal * FACTOR_M2_TO_AREA;
            break;
         case UnitArea.HECTARE:
            val = inVal * FACTOR_M2_TO_HECTARE;
            break;
         case UnitArea.KM2:
            val = inVal * FACTOR_M2_TO_KM2;
            break;
         case UnitArea.FOOT2:
            val = inVal * FACTOR_FOOT2_TO_M2;
            break;
         case UnitArea.YARD2:
            val = inVal * FACTOR_YARD2_TO_M2;
            break;
         case UnitArea.PERCH:
            val = inVal * FACTOR_PERCH_TO_M2;
            break;
         case UnitArea.ACRE:
            val = inVal * FACTOR_ACRE_TO_M2;
            break;
         case UnitArea.MILE2:
            val = inVal * FACTOR_M2_TO_MILE2;
            break;
      }

      //Convert from m2
      switch (toUnit)
      {
         case UnitArea.M2:
            outVal = val;
            break;
         case UnitArea.MM2:
            outVal = val * FACTOR_MM2_TO_M2;
            break;
         case UnitArea.CM2:
            outVal = val * FACTOR_CM2_TO_M2;
            break;
         case UnitArea.AREA:
            outVal = val / FACTOR_M2_TO_AREA;
            break;
         case UnitArea.HECTARE:
            outVal = val / FACTOR_M2_TO_HECTARE;
            break;
         case UnitArea.KM2:
            outVal = val / FACTOR_M2_TO_KM2;
            break;
         case UnitArea.FOOT2:
            outVal = val / FACTOR_FOOT2_TO_M2;
            break;
         case UnitArea.YARD2:
            outVal = val / FACTOR_YARD2_TO_M2;
            break;
         case UnitArea.PERCH:
            outVal = val / FACTOR_PERCH_TO_M2;
            break;
         case UnitArea.ACRE:
            outVal = val / FACTOR_ACRE_TO_M2;
            break;
         case UnitArea.MILE2:
            outVal = val / FACTOR_M2_TO_MILE2;
            break;
      }

      return outVal;
   }
}