namespace ExpressionParser.Main.Tokens
{
    public abstract class ParenthesisToken : Token
    {
        public int Depth { get; set; }

        protected ParenthesisToken(int depth)
        {
            this.Depth = depth;
        }
    }

    public class OpenParenthesisToken : ParenthesisToken
    {
        public OpenParenthesisToken(int depth)
            : base(depth)
        {
        }

        public override string Value
        {
            get { return "("; }
        }
    }

    public class ClosedParenthesisToken : ParenthesisToken
    {
        public ClosedParenthesisToken(int depth)
            : base(depth)
        {
        }

        public override string Value
        {
            get { return ")"; }
        }

    }
}