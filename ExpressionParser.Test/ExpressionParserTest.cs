using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser.Test
{
    [TestClass]
    public class ExpressionParserTest
    {
        [TestMethod]
        public void TestSimpleExpressions()
        {
            var ctxt =
                new ExpressionParserTestContext().WithTestEntities()
                    .UseExpression("StringValue = 'CCC'");
            ctxt.AssertCorrectWhereExpression(x => x.StringValue == "CCC");
            ctxt = ctxt.UseExpression("IntValue <= 3");
            ctxt.AssertCorrectWhereExpression(x => x.IntValue <= 3);
            ctxt = ctxt.UseExpression(string.Format("DateValue >= '{0}'", DateTime.Now));
            ctxt.AssertCorrectWhereExpression(x => x.DateValue >= DateTime.Now);
        }

        [TestMethod]
        public void TestComplexExpressions()
        {
            var ctxt =
                new ExpressionParserTestContext().WithTestEntities(50)
                    .UseExpression("StringValue != 'AAA' AND (IntValue > 2 OR DateValue < '" +
                                   DateTime.Now.ToString("s") + "')");
            ctxt.AssertCorrectWhereExpression(
                x => x.StringValue != "AAA" && (x.IntValue > 2 || x.DateValue < DateTime.Now));
            ctxt = ctxt.UseExpression(string.Format("IntValue > 2 OR DoubleValue <= 5.314 AND DateValue >= '{0}'", DateTime.Now));
            ctxt.AssertCorrectWhereExpression(x => x.IntValue > 2 || x.DoubleValue <= 5.314 && x.DateValue >= DateTime.Now);
        }
    }
}
