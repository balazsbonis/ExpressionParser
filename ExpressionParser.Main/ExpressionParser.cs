using System;

namespace ExpressionParser.Main
{
    public class ExpressionParser<TEntity>
    {
        public string ExpressionToParse { get; set; }
        
        public Func<TEntity, bool> ParseExpression()
        {
            var result = new Func<TEntity, bool>(x => true);

            var tokenizer = new Tokenizer<TEntity>(ExpressionToParse);
            var tokenlist = tokenizer.Tokenize();
            var fb = new FormulaBuilder();
            var formula = fb.BuildFormula(tokenlist);

            return result;
        }
    }
}
