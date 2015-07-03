namespace ExpressionParser.Main.Tokens
{
    public abstract class ParenthesisToken : Token
    {
    }

    public class OpenParenthesisToken : ParenthesisToken
    {
        public override string Value
        {
            get { return "("; }
        }
    }

    public class ClosedParenthesisToken : ParenthesisToken
    {
        public override string Value
        {
            get { return ")"; }
        }
    }
}