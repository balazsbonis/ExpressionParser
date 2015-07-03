namespace ExpressionParser.Main.Tokens
{
    /// <summary>
    /// Predicates work on atoms: =, !=, &gt;, >, &gt;=, >=, and IN is going to come
    /// </summary>
    public abstract class PredicateToken : Token
    {
    }

    public class EqualsToken : PredicateToken
    {
        public override string Value
        {
            get { return "="; }
        }
    }

    public class NotEqualsToken : PredicateToken
    {
        public override string Value
        {
            get { return "!="; }
        }
    }

    public class LessThanToken : PredicateToken
    {
        public override string Value
        {
            get { return "<"; }
        }
    }

    public class LessOrEqualsToken : PredicateToken
    {
        public override string Value
        {
            get { return "<="; }
        }
    }
    public class GreaterThanToken : PredicateToken
    {
        public override string Value
        {
            get { return ">"; }
        }
    }

    public class GreaterOrEqualsToken : PredicateToken
    {
        public override string Value
        {
            get { return ">="; }
        }
    }
}