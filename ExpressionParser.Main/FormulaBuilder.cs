using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionParser.Main.Tokens;

namespace ExpressionParser.Main
{
    public class FormulaBuilder
    {
        public Formula Formula { get; set; }

        public Formula BuildFormula(IEnumerable<Token> tokenList)
        {
            // first we check if we have any parentheses or logical operators
            var tokenArray = tokenList.ToArray();
            if (!tokenArray.Any(x => x is OpenParenthesisToken || x is ClosedParenthesisToken || x is OperatorToken))
            {
                // in case we haven't then it's an atomic formula
                return BuildAtomicFormula(tokenArray);
            }
            // then break up by the parentheses
            var i = 0;
            if (tokenArray[i] is OpenParenthesisToken)
            {
                var tList = new List<Token>();
                var depth = ((ParenthesisToken)tokenArray[i]).Depth;
                while (i < tokenArray.Length)
                {
                    i++;
                    if (tokenArray[i] is ClosedParenthesisToken && ((ParenthesisToken)tokenArray[i]).Depth == depth)
                    {
                        break;
                    }
                    tList.Add(tokenArray[i]);
                    
                }
                return new Formula {Subformula = new List<Formula> {BuildFormula(tList)}};
            }

            // if there are no parentheses found and not an atomic formula, then we must slice at operators
            // left-precedence
            var leftFormula = new List<Token>();
            var rightFormula = new List<Token>();
            while (!(tokenArray[i] is OperatorToken))
            {
                leftFormula.Add(tokenArray[i]);
                i++;
            }
            var logicalOp = (OperatorToken)tokenArray[i];
            i++;
            while (i < tokenArray.Length)
            {
                rightFormula.Add(tokenArray[i]);
                i++;
            }
            return new Formula
            {
                Operator = logicalOp,
                Subformula = new List<Formula>
                {
                    BuildFormula(leftFormula),
                    BuildFormula(rightFormula)
                }
            };
        }

        public Formula BuildAtomicFormula(IEnumerable<Token> tokenList)
        {
            Predicate predicate = null;
            ObjectPropertyToken tempObjProp = null;
            PredicateToken tempPredicate = null;
            var listOfValues = new List<PropertyValueToken>();
            var i = 0;
            var tList = tokenList.ToArray();

            while (i < tList.Length)
            {
                var token = tList[i];
                if (token is ObjectPropertyToken && tempObjProp == null && tempPredicate == null)
                {
                    tempObjProp = (ObjectPropertyToken)token;
                    i++;
                    continue;
                }
                if (token is PredicateToken && tempObjProp != null && tempPredicate == null)
                {
                    tempPredicate = (PredicateToken)token;
                    i++;
                    continue;
                }
                if (token is PropertyValueToken && tempObjProp != null && tempPredicate != null)
                {
                    while (i < tList.Length && tList[i] is PropertyValueToken)
                    {
                        listOfValues.Add((PropertyValueToken)token);
                        i++;
                    }
                    predicate = new Predicate
                    {
                        ObjectProperty = tempObjProp,
                        PredicateType = tempPredicate,
                        PropertyValue = listOfValues
                    };
                    break;
                }
                throw new Exception("Cannot build formula from token list");
            }
            if (i < tList.Length - 1)
            {
                throw new Exception("There are more tokens on the list that could not be parsed");
            }
            return new Formula {Predicate = predicate};
        }
    }
}
