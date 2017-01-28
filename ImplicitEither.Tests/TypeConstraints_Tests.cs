using System;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class TypeConstraints_Tests
    {
        [Test]
        public void Test()
        {
            Assert.Throws<TypeInitializationException>(TryCreateEither<string, string>());
            Assert.DoesNotThrow(TryCreateEither<string, int>());
        }

        private static TestDelegate TryCreateEither<TL, TR>()
        {
            return () =>
            {
                Either<TL, TR>.Create(default(TL));
                Either<TL, TR>.Create(default(TR));
            };
        }
    }
}