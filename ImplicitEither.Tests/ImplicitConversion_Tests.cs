using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class ImplicitConversion_Tests : TestBase
    {
        [Test]
        public void TestInputParameter()
        {
            TakeEitherArgumentType(IntValue);
            TakeEitherArgumentType(StringValue);
            Assert.Throws<AssertionException>(() => TakeEitherArgumentType(new {}));
        }

        private void TakeEitherArgumentType(object x) => Assert.Fail();
        private void TakeEitherArgumentType(Either<int, string> either) => AssertEitherIntOrString(either);

        [Test]
        public void TestReturnValue()
        {
            AssertInt(ReturnEither(intValue: IntValue));
            AssertString(ReturnEither(stringValue: StringValue));
        }

        private Either<int, string> ReturnEither(int? intValue = null, string stringValue = null)
        {
            if (intValue.HasValue)
                return intValue;
            return stringValue;
        }

        [Test]
        public void TestEitherEnumerable()
        {
            TakeEitherParams(EnumerateEither().ToArray());
            TakeEitherParams(EitherInt, EitherString);
        }

        private IEnumerable<Either<int, string>> EnumerateEither()
        {
            yield return IntValue;
            yield return StringValue;
        }

        private void TakeEitherParams(params Either<int, string>[] eithers)
        {
            foreach (var either in eithers)
            {
                AssertEitherIntOrString(either);
            }
        }
    }
}