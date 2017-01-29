using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class UnwrappingNested_Tests : TestBase
    {
        [Test]
        public void Test()
        {
            Assert.Multiple(() =>
            {
                AssertType<int, string>(() => CreateLeft(default(Either<int, Either<int, string>>)).Unwrap());
                AssertType<int, string>(() => CreateLeft(default(Either<Either<int, string>, string>)).Unwrap());

                AssertType<int, string>(() => CreateLeft(default(Either<int, Either<string, int>>)).Unwrap());
                AssertType<int, string>(() => CreateLeft(default(Either<Either<string, int>, string>)).Unwrap());

                AssertType<int, string>(() => CreateLeft(default(Either<Either<int, string>, Either<int, string>>)).Unwrap());
                AssertType<int, string>(() => CreateLeft(default(Either<Either<int, string>, Either<string, int>>)).Unwrap());

                AssertType<int, string>(() => CreateRight(default(Either<int, Either<int, string>>)).Unwrap());
                AssertType<int, string>(() => CreateRight(default(Either<Either<int, string>, string>)).Unwrap());

                AssertType<int, string>(() => CreateRight(default(Either<Either<string, int>, string>)).Unwrap());
                AssertType<int, string>(() => CreateRight(default(Either<int, Either<string, int>>)).Unwrap());

                AssertType<int, string>(() => CreateRight(default(Either<Either<int, string>, Either<int, string>>)).Unwrap());
                AssertType<int, string>(() => CreateRight(default(Either<Either<int, string>, Either<string, int>>)).Unwrap());
            });
        }

        // ReSharper disable InconsistentNaming
        // ReSharper disable UnusedParameter.Local
        private static Either<L, R> CreateLeft<L, R>(Either<L, R> _) => Either<L, R>.Create(default(L));
        private static Either<Either<L, R>, R2> CreateLeft<L, R, R2>(Either<Either<L, R>, R2> _) => Either<Either<L, R>, R2>.Create(default(Either<L, R>));
        private static Either<L, R> CreateRight<L, R>(Either<L, R> _) => Either<L, R>.Create(default(R));
        private static Either<L2, Either<L, R>> CreateRight<L, R, L2>(Either<L2, Either<L, R>> _) => Either<L2, Either<L, R>>.Create(default(Either<L, R>));

        // ReSharper restore UnusedParameter.Local
        // ReSharper restore InconsistentNaming
    }
}