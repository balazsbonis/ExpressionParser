namespace ExpressionParser.Main.Tokens
{
    /// <summary>
    /// Operators connect predicates: AND, OR
    /// </summary>
    public abstract class OperatorToken : Token
    {
        protected int Depth { get; set; }

        protected OperatorToken(int depth)
        {
            Depth = depth;
        }
    }

    public class AndToken : OperatorToken
    {
        public AndToken(int depth)
            : base(depth)
        {
        }

        public override string Value
        {
            get { return "and"; }
        }
    }

    public class OrToken : OperatorToken
    {
        public OrToken(int depth)
            : base(depth)
        {
        }

        public override string Value
        {
            get { return "or"; }
        }
    }
}