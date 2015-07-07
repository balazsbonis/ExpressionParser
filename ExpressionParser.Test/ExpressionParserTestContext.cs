using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser.Test
{
    public class ExpressionParserTestContext
    {
        private List<TestEntity> testEntitiesList;
        private Main.ExpressionParser<TestEntity> testParser;
        public Func<TestEntity, bool> clause;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        public ExpressionParserTestContext()
        {
            testEntitiesList = new List<TestEntity>();
            testParser = new Main.ExpressionParser<TestEntity>();

        }

        public ExpressionParserTestContext WithTestEntities(int number = 20)
        {
            var random = new Random();
            for (var i = 0; i < number; i++)
            {
                testEntitiesList.Add(new TestEntity
                {
                    DateValue = DateTime.Now.AddMinutes((random.NextDouble() - 0.5) * 15),
                    IntValue = random.Next(-5, 5),
                    StringValue = random.Next(0, 3) == 1 ? "AAA" : random.Next(0, 3) == 1 ? "BBB" : "CCC",
                    OtherProperty = Guid.NewGuid(),
                    DoubleValue = random.NextDouble() * 10
                });
            }
            return this;
        }

        public ExpressionParserTestContext UseExpression(string expression)
        {
            testParser.ExpressionToParse = expression;
            clause = testParser.ParseExpression();
            return this;
        }

        public void AssertCorrectWhereExpression(Func<TestEntity, bool> lambdaExpression)
        {
            Debug.WriteLine(testParser.ExpressionToParse);
            var testAgainst = testEntitiesList.Where(lambdaExpression).ToList();
            var testCases = testEntitiesList.Where(clause).ToList();
            foreach (var t in testAgainst)
            {
                Debug.WriteLine("Check: {0}", t.OtherProperty);
                Assert.IsTrue(testCases.Contains(t));
            }
            Assert.AreEqual(testAgainst.Count(), testCases.Count());
        }
    }
    public class TestEntity
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public Guid OtherProperty { get; set; }
        public DateTime DateValue { get; set; }
        public double DoubleValue { get; set; }
    }
}
