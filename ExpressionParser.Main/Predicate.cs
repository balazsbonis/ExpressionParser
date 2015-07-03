using System.Collections.Generic;
using ExpressionParser.Main.Tokens;

namespace ExpressionParser.Main
{
    public class Predicate
    {
        public PredicateToken PredicateType { get; set; }
        public ObjectPropertyToken ObjectProperty { get; set; }
        public IEnumerable<PropertyValueToken> PropertyValue { get; set; }
    }
}