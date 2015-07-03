using System;
using System.Reflection;

namespace ExpressionParser.Main.Tokens
{
    public abstract class Token
    {
        public abstract string Value { get; }
    }

    public class ObjectPropertyToken : Token
    {
        public override string Value
        {
            get { return ""; }
        }

        public PropertyInfo ObjectProperty { get; set; }

        public ObjectPropertyToken(PropertyInfo objectProperty)
        {
            ObjectProperty = objectProperty;
        }
    }

    public class PropertyValueToken : Token
    {
        public override string Value
        {
            get { return ""; }
        }

        public string ObjectValue { get; set; }

        public PropertyValueToken(string value)
        {
            ObjectValue = value;
        }
    }
}