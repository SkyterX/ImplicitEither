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
    }
}