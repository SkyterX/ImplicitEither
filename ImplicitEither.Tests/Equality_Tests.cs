using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class Equality_Tests : TestBase
    {
        [Test]
        public void TestInteger()
        {
            Either<int, string> eitherInt = IntValue;
            Assert.That(eitherInt, Is.EqualTo(EitherInt));
            Assert.That(eitherInt.GetHashCode(), Is.EqualTo(EitherInt.GetHashCode()));
            Assert.That(eitherInt == EitherInt);

            eitherInt = IntValue + 1;
            Assert.That(eitherInt, Is.Not.EqualTo(EitherInt));
            Assert.That(eitherInt != EitherInt);
        }

        [Test]
        public void TestIntString()
        {
            Assert.That(EitherString, Is.Not.EqualTo(EitherInt));
            Assert.That(EitherString != EitherInt);
        }

        [Test]
        public void TestString()
        {
            Either<int, string> eitherString = StringValue;
            Assert.That(eitherString, Is.EqualTo(EitherString));
            Assert.That(eitherString.GetHashCode(), Is.EqualTo(EitherString.GetHashCode()));
            Assert.That(eitherString == EitherString);

            eitherString = StringValue + "1";
            Assert.That(eitherString, Is.Not.EqualTo(EitherString));
            Assert.That(eitherString != EitherString);
        }

        [Test]
        public void TestHashCode()
        {
            var eithers = new List<Either<int, string>>
            {
                EitherInt,
                EitherString,
                IntValue + 1,
                IntValue,
                StringValue + "1",
                StringValue
            };
            Assert.That(eithers, Is.Not.Unique);
            Assert.That(eithers.Distinct(), Is.Unique);
            Assert.That(eithers.Distinct(), Has.Exactly(4).Items);

            var eithersDict = eithers.Distinct().ToDictionary(x => x);
            Assert.That(eithersDict, Contains.Key(EitherInt));
            Assert.That(eithersDict[EitherInt], Is.EqualTo(EitherInt));
            Assert.That(eithersDict, Contains.Key(EitherString));
            Assert.That(eithersDict[EitherString], Is.EqualTo(EitherString));
        }
    }
}