using System;

namespace ExpressionParser.Main
{
    public class ExpressionParser<TEntity>
    {
        public string ExpressionToParse { get; set; }

        private string ValidateExpression()
        {
            return string.Empty;
        }

        public Func<TEntity, bool> ParseExpression()
        {
            var result = new Func<TEntity, bool>(x => true);

            var tokenizer = new Tokenizer<TEntity>(ExpressionToParse);
            var tokenlist = tokenizer.Tokenize();

            return result;
        }
    }
}
