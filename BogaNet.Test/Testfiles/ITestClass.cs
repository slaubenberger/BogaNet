namespace BogaNet.Test.Testfiles;

/// <summary>
/// Interface for the dummy class.
/// </summary>
public interface ITestClass
{
   string PublicProp { get; set; }

   void PrintPublicString(string prefix = "");
   void PrintPrivateString(string prefix = "");
}