using System.Collections.Generic;
using ExpressionParser.Main.Tokens;

namespace ExpressionParser.Main
{
    public class Formula
    {
        public Predicate Predicate { get; set; }
        public OperatorToken Operator { get; set; }
        public IEnumerable<Formula> Subformula { get; set; }
    }
}