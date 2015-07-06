using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionParser.Main.Tokens;

namespace ExpressionParser.Main
{
    public class ExpressionParser<TEntity> where TEntity : new()
    {
        public string ExpressionToParse { get; set; }

        public TEntity SampleEntity { get; set; }

        public Func<TEntity, bool> ParseExpression()
        {
            var result = new Func<TEntity, bool>(x => true);

            SampleEntity = new TEntity();

            var tokenizer = new Tokenizer<TEntity>(ExpressionToParse);
            var tokenlist = tokenizer.Tokenize();
            var formula = new FormulaBuilder().BuildFormula(tokenlist);
            
            var param = Expression.Parameter(typeof(TEntity), "entity");

            var expr = BuildLambda(formula, param);
            
            result =
                Expression.Lambda<Func<TEntity, bool>>(expr,
                    new[] { param }).Compile();
            return result;
        }

        private Expression BuildLambda(Formula formula, ParameterExpression parameter)
        {
            if (formula.Subformula != null && formula.Operator != null)
            {
                if (formula.Operator is AndToken)
                {
                    return Expression.And(BuildLambda(formula.Subformula.First(), parameter),
                        BuildLambda(formula.Subformula.Last(), parameter));
                }
                if (formula.Operator is OrToken)
                {
                    return Expression.Or(BuildLambda(formula.Subformula.First(), parameter),
                        BuildLambda(formula.Subformula.Last(), parameter));
                }
            }
            if (formula.Predicate != null)
            {
                var property = Expression.PropertyOrField(parameter,
                    formula.Predicate.ObjectProperty.ObjectProperty.Name);
                var value =
                    Expression.Constant(TryParseObject(property.Type,
                        formula.Predicate.PropertyValue.First().ObjectValue));
                
                if (formula.Predicate.PredicateType is EqualsToken)
                {
                    return Expression.Equal(property,value);
                }
                if (formula.Predicate.PredicateType is NotEqualsToken)
                {
                    return Expression.NotEqual(property, value);
                } 
                if (formula.Predicate.PredicateType is LessOrEqualsToken)
                {
                    return Expression.LessThanOrEqual(property, value);
                }
                if (formula.Predicate.PredicateType is LessThanToken)
                {
                    return Expression.LessThan(property, value);
                }
                if (formula.Predicate.PredicateType is GreaterOrEqualsToken)
                {
                    return Expression.GreaterThanOrEqual(property, value);
                }
                if (formula.Predicate.PredicateType is GreaterThanToken)
                {
                    return Expression.GreaterThan(property, value);
                }
            }
            throw new Exception("Could not build lambda expression from tree.");
        }

        private object TryParseObject(Type propertyType, string value)
        {
            var methodlist = propertyType.GetMethods().Where(x => x.Name == "Parse").ToArray();
            if (methodlist.Any())
            {
                var result = methodlist[0].Invoke(value, new[] {value});
                if (result != null)
                {
                    return result;
                }
            }
            return value;
        }
    }
}
