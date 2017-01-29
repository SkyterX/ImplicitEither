using System;
using JetBrains.Annotations;

// ReSharper disable InconsistentNaming

namespace ImplicitEither
{
    public class Either<L, R>
    {
        internal L Left { get; }

        internal R Right { get; }

        internal bool IsLeft { get; }
        internal bool IsRight => !IsLeft;

        private Either([CanBeNull] L left, [CanBeNull] R right, bool isLeft)
        {
            Left = left;
            Right = right;
            IsLeft = isLeft;
        }

        [NotNull]
        public static Either<L, R> Create([CanBeNull] L left) => new Either<L, R>(left, default(R), isLeft: true);

        [NotNull]
        public static Either<L, R> Create([CanBeNull] R right) => new Either<L, R>(default(L), right, isLeft: false);

        [NotNull]
        public static implicit operator Either<L, R>([CanBeNull] L left) => Create(left);

        [NotNull]
        public static implicit operator Either<L, R>([CanBeNull] R right) => Create(right);

        [NotNull]
        public static implicit operator Either<L, R>([NotNull] Either<R, L> either) => either.Reverse();
    }

    public static class EitherExtensions
    {
        public static L Left<L, R>([NotNull] this Either<L, R> either, Func<R, L> right = null)
        {
            if (either.IsLeft)
                return either.Left;
            if (right != null)
                return right(either.Right);
            throw new ArgumentException($"Either is {typeof(R).Name}, but no suitable conversion to {typeof(L).Name} provided.", nameof(either));
        }

        public static R Right<L, R>([NotNull] this Either<L, R> either, Func<L, R> left = null)
        {
            if (either.IsRight)
                return either.Right;
            if (left != null)
                return left(either.Left);
            throw new ArgumentException($"Either is {typeof(L).Name}, but no suitable conversion to {typeof(R).Name} provided.", nameof(either));
        }

        public static TResult Return<L, R, TResult>([NotNull] this Either<L, R> either, Func<L, TResult> left, Func<R, TResult> right)
        {
            return either.IsLeft ? left(either.Left) : right(either.Right);
        }

        public static void Do<L, R>([NotNull] this Either<L, R> either, Action<L> left = null, Action<R> right = null)
        {
            if (either.IsLeft)
                left?.Invoke(either.Left);
            else
                right?.Invoke(either.Right);
        }

        public static void IfLeft<L, R>([NotNull] this Either<L, R> either, [NotNull] Action<L> left) => either.Do(left: left);
        public static void IfRight<L, R>([NotNull] this Either<L, R> either, [NotNull] Action<R> right) => either.Do(right: right);

        [NotNull]
        public static Either<L, R> Reverse<L, R>([NotNull] this Either<R, L> either)
        {
            if (either.IsLeft)
                return either.Left;
            return either.Right;
        }

        #region Mapping left either

        [NotNull]
        public static Either<NL, R> MapLeft<L, R, NL>([NotNull] this Either<L, R> either, [NotNull] Func<L, NL> left, Action<R> right = null)
            => either.MapLeft(l => (Either<NL, R>) left(l), right);

        [NotNull]
        public static Either<NL, R> MapLeft<L, R, NL>([NotNull] this Either<L, R> either, [NotNull] Func<L, Either<R, NL>> left, Action<R> right = null)
            => either.MapLeft(l => (Either<NL, R>) left(l), right);

        [NotNull]
        public static Either<NL, R> MapLeft<L, R, NL>(
            [NotNull] this Either<L, R> either,
            [NotNull] Func<L, Either<NL, R>> left,
            Action<R> right = null)
        {
            if (either.IsLeft)
                return left(either.Left);
            right?.Invoke(either.Right);
            return either.Right;
        }

        #endregion

        #region Mapping right either

        [NotNull]
        public static Either<L, NR> MapRight<L, R, NR>([NotNull] this Either<L, R> either, [NotNull] Func<R, NR> right, Action<L> left = null)
            => either.MapRight(r => (Either<L, NR>) right(r), left);

        [NotNull]
        public static Either<L, NR> MapRight<L, R, NR>([NotNull] this Either<L, R> either, [NotNull] Func<R, Either<NR, L>> right, Action<L> left = null)
            => either.MapRight(r => (Either<L, NR>) right(r), left);

        [NotNull]
        public static Either<L, NR> MapRight<L, R, NR>(
            [NotNull] this Either<L, R> either,
            [NotNull] Func<R, Either<L, NR>> right,
            Action<L> left = null)
        {
            if (either.IsRight)
                return right(either.Right);
            left?.Invoke(either.Left);
            return either.Left;
        }

        #endregion

        #region Mapping either

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, NL> left, [NotNull] Func<R, NR> right)
            => either.Map(l => (Either<NL, NR>) left(l), r => (Either<NL, NR>) right(r));

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, Either<NL, NR>> left, [NotNull] Func<R, NR> right)
            => either.Map(left, r => (Either<NL, NR>) right(r));

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, NL> left, [NotNull] Func<R, Either<NR, NL>> right)
            => either.Map(l => (Either<NL, NR>) left(l), r => (Either<NL, NR>) right(r));

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, Either<NR, NL>> left, [NotNull] Func<R, NR> right)
            => either.Map(l => (Either<NL, NR>) left(l), r => (Either<NL, NR>) right(r));

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, NL> left, [NotNull] Func<R, Either<NL, NR>> right)
            => either.Map(l => (Either<NL, NR>) left(l), right);

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>([NotNull] this Either<L, R> either, [NotNull] Func<L, Either<NL, NR>> left,
                                                       [NotNull] Func<R, Either<NR, NL>> right)
            => either.Map(left, r => (Either<NL, NR>) right(r));

        [NotNull]
        public static Either<NL, NR> Map<L, R, NL, NR>(
            [NotNull] this Either<L, R> either,
            [NotNull] Func<L, Either<NL, NR>> left,
            [NotNull] Func<R, Either<NL, NR>> right)
        {
            if (either.IsLeft)
                return left(either.Left);
            return right(either.Right);
        }

        #endregion

        #region Unwrapping nested either

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<L, Either<L, R>> either) => either.IsLeft ? either.Left : either.Right;

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<L, Either<R, L>> either) => either.IsLeft ? either.Left : either.Right;

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<Either<L, R>, R> either) => either.IsLeft ? either.Left : either.Right;

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<Either<R, L>, R> either) => either.IsLeft ? either.Left : either.Right;

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<Either<L, R>, Either<L, R>> either) => either.IsLeft ? either.Left : either.Right;

        [NotNull]
        public static Either<L, R> Unwrap<L, R>([NotNull] this Either<Either<L, R>, Either<R, L>> either)
            => either.IsLeft ? either.Left : (Either<L, R>) either.Right;

        #endregion
    }
}