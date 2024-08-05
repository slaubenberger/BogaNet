using System;
using BogaNet.Extension;

namespace BogaNet.Test.Testfiles;

/// <summary>
/// Dummy class to simulate an actual object for NUnit tests.
/// </summary>
public class TestClass : ITestClass
{
   #region Variables

   public string PublicString = "PublicString";
   public static string PublicStaticString = "PublicStaticString";

   private string _privateString = "privateString";
   private static string _privateStaticString = "privateStaticString";

   #endregion

   #region Properties

   public string PublicProp { get; set; } = "PublicProp";
   public static string PublicStaticProp { get; set; } = "PublicStaticProp";

   private string privateProp { get; set; } = "privateProp";
   private static string privateStaticProp { get; set; } = "privateStaticProp";

   #endregion

   #region Constructors

   public TestClass()
   {
   }

   public TestClass(string privateString)
   {
      _privateString = privateString;
   }

   #endregion

   #region Public methods

   public void PrintPublicString(string prefix = "")
   {
      Console.WriteLine($"{prefix}{PublicString}");
   }

   public void PrintPrivateString(string prefix = "")
   {
      Console.WriteLine($"{prefix}{_privateString}");
   }

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      return this.BNToString();
   }

   public override bool Equals(object? obj)
   {
      if (obj == null || GetType() != obj.GetType())
         return false;

      TestClass testClass = (TestClass)obj;

      return PublicString.Equals(testClass.PublicString) &&
             _privateString.Equals(testClass._privateString) &&
             PublicProp.Equals(testClass.PublicProp) &&
             privateProp.Equals(testClass.privateProp);
   }

   public override int GetHashCode()
   {
      return base.GetHashCode();
   }

   #endregion
}