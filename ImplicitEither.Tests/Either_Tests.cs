using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class Either_Tests : TestBase
    {
        [Test]
        public void TestGetLeft()
        {
            AssertInt(EitherInt.Left());
            Assert.Catch(() => EitherString.Left());
            AssertInt(EitherString.Left(right: int.Parse));
        }

        [Test]
        public void TestGetRight()
        {
            AssertString(EitherString.Right());
            Assert.Catch(() => EitherInt.Right());
            AssertString(EitherInt.Right(left: l => l.ToString()));
        }

        [Test]
        public void TestReturn()
        {
            AssertInt(EitherString.Return(left: l => l, right: int.Parse));
            AssertString(EitherInt.Return(left: l => l.ToString(), right: r => r));
        }

        [Test]
        public void TestDo()
        {
            EitherInt.Do(left: AssertInt, right: r => Assert.Fail());
            EitherInt.IfLeft(AssertInt);
            EitherString.Do(left: l => Assert.Fail(), right: AssertString);
            EitherString.IfRight(AssertString);
        }
    }
}