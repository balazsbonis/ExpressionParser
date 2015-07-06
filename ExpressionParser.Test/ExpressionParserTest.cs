using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser.Test
{
    [TestClass]
    public class ExpressionParserTest
    {
        private List<TestEntity> testEntitiesList;
        private Main.ExpressionParser<TestEntity> testParser;

        public ExpressionParserTest()
        {
            testEntitiesList = new List<TestEntity>();
            testParser = new Main.ExpressionParser<TestEntity>();
            var random = new Random();
            for (var i = 0; i < 20; i++)
            {
                testEntitiesList.Add(new TestEntity
                {
                    DateValue = DateTime.Now.AddMinutes((random.NextDouble() - 0.5) * 15),
                    IntValue = random.Next(-5, 5),
                    StringValue = random.Next(0,2) == 1 ? "AAA" : "BBB",
                    OtherProperty = Guid.NewGuid().ToString().Replace("-", "")
                });
            }
        }

        [TestMethod]
        public void TestSelectionEqual()
        {
            testParser.ExpressionToParse = "StringValue = 'AAA'";
            var clause = testParser.ParseExpression();
        }

        [TestMethod]
        public void TestComplexExpressions()
        {
            testParser.ExpressionToParse = "StringValue = 'AAA' AND (IntValue > '3' OR OtherProperty = 'CCC')";
            var testAgainst =
                testEntitiesList.Where(x => x.StringValue == "AAA" && (x.IntValue > 3 || x.OtherProperty == "CCC"));
            var clause = testParser.ParseExpression();
            var testCases = testEntitiesList.Where(clause);
        }
    }

    public class TestEntity
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public string OtherProperty { get; set; }
        public DateTime DateValue { get; set; }
    }
}
