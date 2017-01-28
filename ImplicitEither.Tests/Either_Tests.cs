using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class Either_Tests
    {
        private const string stringValue = "0";
        private const int intValue = 0;

        private Either<int, string> EitherInt { get; } = Either<int, string>.Create(intValue);
        private Either<int, string> EitherString { get; } = Either<int, string>.Create(stringValue);


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
            EitherString.Do(left: l => Assert.Fail(), right: AssertString);
        }

        private void AssertInt(int x) => Assert.That(x, Is.EqualTo(intValue));
        private void AssertString(string x) => Assert.That(x, Is.EqualTo(stringValue));
    }
}