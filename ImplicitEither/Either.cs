namespace ImplicitEither
{
    public class Either<L, R>
    {
        public L Left { get; }
        public R Right { get; }
        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;

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
}