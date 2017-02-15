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
                AssertType<int, string>(() => CreateLeft<int, Either<int, string>>().Unwrap());
                AssertType<int, string>(() => CreateLeft<Either<int, string>, string>().Unwrap());

                AssertType<int, string>(() => CreateLeft<int, Either<string, int>>().Unwrap());
                AssertType<int, string>(() => CreateLeft<Either<string, int>, string>().Unwrap());

                AssertType<int, string>(() => CreateLeft<Either<int, string>, Either<int, string>>().Unwrap());
                AssertType<int, string>(() => CreateLeft<Either<int, string>, Either<string, int>>().Unwrap());

                AssertType<int, string>(() => CreateRight<int, Either<int, string>>().Unwrap());
                AssertType<int, string>(() => CreateRight<Either<int, string>, string>().Unwrap());

                AssertType<int, string>(() => CreateRight<Either<string, int>, string>().Unwrap());
                AssertType<int, string>(() => CreateRight<int, Either<string, int>>().Unwrap());

                AssertType<int, string>(() => CreateRight<Either<int, string>, Either<int, string>>().Unwrap());
                AssertType<int, string>(() => CreateRight<Either<int, string>, Either<string, int>>().Unwrap());
            });
        }
    }
}