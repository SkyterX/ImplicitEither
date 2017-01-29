using System;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class MappingEither_Simplify_Tests : TestBase
    {
        [Test]
        public void TestMapLeft()
        {
            Assert.Multiple(() =>
            {
                AssertSimplified(() => EitherInt.MapLeft(l => l));
                AssertSimplified(() => EitherInt.MapLeft(l => Return_Either(l))); // Either<Either<L, R>, R> -> Either<L, R>
                AssertSimplified(() => EitherInt.MapLeft(l => ReverseEither(l))); // Either<Either<R, L>, R> -> Either<L, R>
            });
        }

        [Test]
        public void TestMapRight()
        {
            Assert.Multiple(() =>
            {
                AssertSimplified(() => EitherString.MapRight(r => r));
                AssertSimplified(() => EitherString.MapRight(r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>
                AssertSimplified(() => EitherString.MapRight(r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>
            });
        }

        [Test]
        public void TestMap()
        {
            Assert.Multiple(() =>
            {
                foreach (var either in new[] {EitherInt, EitherString})
                {
                    AssertSimplified(() => either.Map(l => l, r => r));
                    AssertSimplified(() => either.Map(l => Return_Either(l), r => r)); // Either<Either<L, R>, R> -> Either<L, R>
                    AssertSimplified(() => either.Map(l => l, r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>

                    AssertSimplified(() => either.Map(l => ReverseEither(l), r => r)); // Either<Either<R, L>, R> -> Either<L, R>
                    AssertSimplified(() => either.Map(l => l, r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>

                    AssertSimplified(() => either.Map(l => Return_Either(l), r => Return_Either(r))); // Either<Either<L, R>, Either<L, R>> -> Either<L, R>
                    AssertSimplified(() => either.Map(l => Return_Either(l), r => ReverseEither(r))); // Either<Either<L, R>, Either<R, L>> -> Either<L, R>
                }
            });
        }

        private Either<int, string> Return_Either(Either<int, string> either)
        {
            return either;
        }

        private Either<string, int> ReverseEither(Either<int, string> either)
        {
            return either;
        }

        private static void AssertSimplified<X, Y>(Func<Either<X, Y>> getEither)
        {
            Either<X, Y> either = null;
            Assert.DoesNotThrow(() => either = getEither());
            Assert.That(either, Is.TypeOf<Either<int, string>>());
        }
    }
}