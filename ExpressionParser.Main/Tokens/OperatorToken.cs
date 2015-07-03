namespace ExpressionParser.Main.Tokens
{
    /// <summary>
    /// Operators connect predicates: AND, OR
    /// </summary>
    public abstract class OperatorToken : Token
    {
    }

    public class AndToken : OperatorToken
    {
        public override string Value
        {
            get { return "and"; }
        }
    }

    public class OrToken : OperatorToken
    {
        public override string Value
        {
            get { return "or"; }
        }
    }
}