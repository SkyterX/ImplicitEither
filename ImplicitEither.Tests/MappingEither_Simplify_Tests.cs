using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class MappingEither_Simplify_Tests : TestBase
    {
        [Test]
        public void TestMapLeft()
        {
            AssertSimplified(EitherInt.MapLeft(left: l => l));
            AssertSimplified(EitherInt.MapLeft(left: l => Return_Either(l))); // Either<Either<L, R>, R> -> Either<L, R>
            AssertSimplified(EitherInt.MapLeft(left: l => ReverseEither(l))); // Either<Either<R, L>, R> -> Either<L, R>
        }

        [Test]
        public void TestMapRight()
        {
            AssertSimplified(EitherString.MapRight(right: r => r));
            AssertSimplified(EitherString.MapRight(right: r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>
            AssertSimplified(EitherString.MapRight(right: r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>
        }

        [Test]
        public void TestMap()
        {
            foreach (var either in new[] {EitherInt, EitherString})
            {
                AssertSimplified(either.Map(left: l => l, right: r => r));
                AssertSimplified(either.Map(left: l => Return_Either(l), right: r => r)); // Either<Either<L, R>, R> -> Either<L, R>
                AssertSimplified(either.Map(left: l => l, right: r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>

                AssertSimplified(either.Map(left: l => ReverseEither(l), right: r => r)); // Either<Either<R, L>, R> -> Either<L, R>
                AssertSimplified(either.Map(left: l => l, right: r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>

                AssertSimplified(either.Map(left: l => Return_Either(l), right: r => Return_Either(r))); // Either<Either<L, R>, Either<L, R>> -> Either<L, R>
                AssertSimplified(either.Map(left: l => Return_Either(l), right: r => ReverseEither(r))); // Either<Either<L, R>, Either<R, L>> -> Either<L, R>
            }
        }

        private void AssertSimplified<L, R>(Either<L, R> either)
        {
            Assert.That(either, Is.TypeOf<Either<int, string>>());
        }

        private Either<int, string> Return_Either(Either<int, string> either)
        {
            return either;
        }

        private Either<string, int> ReverseEither(Either<int, string> either)
        {
            return either.MapLeft(left: l => (Either<string, int>) l)
                         .Left(right: r => (Either<string, int>) r);
        }
    }
}