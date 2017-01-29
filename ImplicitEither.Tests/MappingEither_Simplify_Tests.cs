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
                AssertSimplified(EitherInt, e => e.MapLeft(l => l));
                AssertSimplified(EitherInt, e => e.MapLeft(l => Return_Either(l))); // Either<Either<L, R>, R> -> Either<L, R>
                AssertSimplified(EitherInt, e => e.MapLeft(l => ReverseEither(l))); // Either<Either<R, L>, R> -> Either<L, R>
            });
        }

        [Test]
        public void TestMapRight()
        {
            Assert.Multiple(() =>
            {
                AssertSimplified(EitherString, e => e.MapRight(r => r));
                AssertSimplified(EitherString, e => e.MapRight(r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>
                AssertSimplified(EitherString, e => e.MapRight(r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>
            });
        }

        [Test]
        public void TestMap()
        {
            Assert.Multiple(() =>
            {
                foreach (var either in new[] {EitherInt, EitherString})
                {
                    AssertSimplified(either, e => e.Map(l => l, r => r));
                    AssertSimplified(either, e => e.Map(l => Return_Either(l), r => r)); // Either<Either<L, R>, R> -> Either<L, R>
                    AssertSimplified(either, e => e.Map(l => l, r => Return_Either(r))); // Either<L, Either<L, R>> -> Either<L, R>

                    AssertSimplified(either, e => e.Map(l => ReverseEither(l), r => r)); // Either<Either<R, L>, R> -> Either<L, R>
                    AssertSimplified(either, e => e.Map(l => l, r => ReverseEither(r))); // Either<L, Either<R, L>> -> Either<L, R>

                    AssertSimplified(either, e => e.Map(l => Return_Either(l), r => Return_Either(r))); // Either<Either<L, R>, Either<L, R>> -> Either<L, R>
                    AssertSimplified(either, e => e.Map(l => Return_Either(l), r => ReverseEither(r))); // Either<Either<L, R>, Either<R, L>> -> Either<L, R>
                }
            });
        }

        // ReSharper disable InconsistentNaming
        private void AssertSimplified<L, R>(Either<L, R> either, Func<Either<L, R>, object> getEither)

        {
            AssertType<L, R>(() => getEither(either));
        }

        // ReSharper restore InconsistentNaming
    }
}