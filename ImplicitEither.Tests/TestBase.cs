using System;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    public abstract class TestBase
    {
        protected const string StringValue = "0";
        protected const int IntValue = 0;
        protected Either<int, string> EitherInt { get; } = Either<int, string>.Create(IntValue);
        protected Either<int, string> EitherString { get; } = Either<int, string>.Create(StringValue);
        protected void AssertInt(int x) => Assert.That(x, Is.EqualTo(IntValue));
        protected void AssertString(string x) => Assert.That(x, Is.EqualTo(StringValue));

        protected void AssertInt(Either<int, string> either) => either.Do(AssertInt, r => Assert.Fail());
        protected void AssertString(Either<int, string> either) => either.Do(l => Assert.Fail(), AssertString);
        protected void AssertEitherIntOrString(Either<int, string> either) => either.Do(AssertInt, AssertString);

        // ReSharper disable InconsistentNaming
        protected static void AssertType<L, R>(Func<object> getEither)

        {
            object either = null;
            Assert.DoesNotThrow(() => either = getEither());
            Assert.That(either, Is.TypeOf<Either<L, R>>());
        }

        protected static Either<L, R> CreateLeft<L, R>() => default(L);
        protected static Either<L, R> CreateRight<L, R>() => default(R);

        // ReSharper restore InconsistentNaming

        protected Either<int, string> Return_Either(Either<int, string> either) => either;
        protected Either<string, int> ReverseEither(Either<int, string> either) => either;
    }
}