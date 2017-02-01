using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class Formatting_Tests : TestBase
    {
        [Test]
        public void Test()
        {
            Assert.That(EitherInt.ToString(), Is.EqualTo($"Int32: {IntValue}"));
            Assert.That(EitherString.ToString(), Is.EqualTo($"String: {StringValue}"));
        }
    }
}