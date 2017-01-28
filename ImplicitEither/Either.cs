using System;

namespace ImplicitEither
{
    public class Either<L, R>
    {
        static Either()
        {
            if (typeof(L) == typeof(R))
                throw new Exception(
                    $"Using the type Either with equal type parameters ({typeof(L).Name}) is prohibited.");
        }

        internal L Left { get; }
        internal R Right { get; }
        internal bool IsLeft { get; }
        internal bool IsRight => !IsLeft;

        private Either(L left, R right, bool isLeft)
        {
            Left = left;
            Right = right;
            IsLeft = isLeft;
        }

        public static Either<L, R> Create(L left)
        {
            return new Either<L, R>(left, default(R), isLeft: true);
        }

        public static Either<L, R> Create(R right)
        {
            return new Either<L, R>(default(L), right, isLeft: false);
        }
    }

    public static class EitherExtensions
    {
        public static L Left<L, R>(this Either<L, R> either, Func<R, L> right = null)
        {
            if (either.IsLeft)
                return either.Left;
            if (right != null)
                return right(either.Right);
            throw new ArgumentException($"Either is {typeof(R).Name}, but no suitable conversion to {typeof(L).Name} provided.", nameof(either));
        }

        public static R Right<L, R>(this Either<L, R> either, Func<L, R> left = null)
        {
            if (either.IsRight)
                return either.Right;
            if (left != null)
                return left(either.Left);
            throw new ArgumentException($"Either is {typeof(L).Name}, but no suitable conversion to {typeof(R).Name} provided.", nameof(either));
        }

        public static TResult Return<L, R, TResult>(this Either<L, R> either, Func<L, TResult> left, Func<R, TResult> right)
        {
            return either.IsLeft ? left(either.Left) : right(either.Right);
        }

        public static void Do<L, R>(this Either<L, R> either, Action<L> left = null, Action<R> right = null)
        {
            if (left != null && either.IsLeft)
                left(either.Left);
            if (right != null && either.IsRight)
                right(either.Right);
        }
    }
}