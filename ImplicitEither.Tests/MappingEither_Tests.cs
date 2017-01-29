using System.Text;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class MappingEither_Tests : TestBase
    {
        [Test]
        public void TestMapLeft()
        {
            Either<int, string> either = IntValue + 1;
            var mappedEither = either.MapLeft(left: l => l - 1, right: r => Assert.Fail());
            Assert.That(mappedEither, Is.TypeOf<Either<int, string>>());
            AssertInt(mappedEither);
            var mappedEither2 = either.MapLeft(left: l => l * 2.0m);
            Assert.That(mappedEither2, Is.TypeOf<Either<decimal, string>>());

            Assert.Throws<AssertionException>(() => EitherString.MapLeft(left: l => l, right: r => Assert.Fail()));
        }

        [Test]
        public void TestMapRight()
        {
            Either<int, string> either = StringValue + "1";
            var mappedEither = either.MapRight(left: l => Assert.Fail(), right: r => r.Substring(0, 1));
            Assert.That(mappedEither, Is.TypeOf<Either<int, string>>());
            AssertString(mappedEither);
            var mappedEither2 = either.MapRight(right: r => new StringBuilder(r));
            Assert.That(mappedEither2, Is.TypeOf<Either<int, StringBuilder>>());

            Assert.Throws<AssertionException>(() => EitherInt.MapRight(left: l => Assert.Fail(), right: r => r));
        }

        [Test]
        public void TestMapSimple()
        {
            Either<int, string> either = IntValue + 1;
            var mappedEither = either.Map(left: l => l - 1, right: r => r.Substring(0, 1));
            Assert.That(mappedEither, Is.TypeOf<Either<int, string>>());
            AssertEitherIntOrString(mappedEither);

            either = StringValue + "1";
            var mappedEither2 = either.Map(left: l => l - 1, right: r => r.Substring(0, 1));
            Assert.That(mappedEither2, Is.TypeOf<Either<int, string>>());
            AssertEitherIntOrString(mappedEither2);

            var mappedEither3 = either.Map(left: l => l * 2.0m, right: r => new StringBuilder(r));
            Assert.That(mappedEither3, Is.TypeOf<Either<decimal, StringBuilder>>());
        }
    }
}