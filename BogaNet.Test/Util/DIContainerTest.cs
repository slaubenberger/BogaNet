using BogaNet.Util;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.Util;

public class DIContainerTest
{
   [Test]
   public void DIContainer_Test()
   {
      TestClass tc = new()
      {
         PublicProp = "Hello",
         PublicString = "Wörld"
      };

      DIContainer.Bind<ITestClass, TestClass>(tc);
      ITestClass res = DIContainer.Resolve<ITestClass>();
      Assert.That(res, Is.EqualTo(tc));

      DIContainer.Unbind<ITestClass>();
      res = DIContainer.Resolve<ITestClass>();

      Assert.That(res, Is.Null);
   }
}