using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DynamicProxy;

namespace StepOnThat.Infrastructure
{
    internal class PropertyInterceptor : IInterceptor
    {
        private const string PropertyMatcher = @"(\$\{[A-z0-9.-]+\})|(\{\{[A-z0-9.-]+\}\})";
        private readonly IHasProperties properties;

        public PropertyInterceptor(IHasProperties properties)
        {
            this.properties = properties;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (IsGetterProperty(invocation.Method)
                && IsPublicStringReturnType(invocation.Method)
                && ReturnsAVariable(invocation))
            {
                if (invocation.ReturnValue != null)
                {
                    var originalValue = invocation.ReturnValue as string;
                    var newValue = ApplyPropertiesToValue(originalValue);
                    invocation.ReturnValue = newValue;
                }
            }
        }

        public string ApplyPropertiesToValue(string originalValue)
        {
            var newValue = Regex.Replace(originalValue, PropertyMatcher, match =>
            {
                var name = Regex.Replace(match.Value, @"\$|{|}", "");
                try
                {
                    return properties[name];
                }
                catch (KeyNotFoundException)
                {
                    throw new ApplicationException(String.Format("Could not find variable with name {0}", name));
                }
            });

            return newValue;
        }

        private static bool ReturnsAVariable(IInvocation invocation)
        {
            var result = invocation.ReturnValue as string;

            return !string.IsNullOrEmpty(result) && Regex.IsMatch(result, PropertyMatcher);
        }

        private static bool IsGetterProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }

        private static bool IsPublicStringReturnType(MethodInfo method)
        {
            return method.IsPublic && (method.ReturnType == typeof (string));
        }
    }
}