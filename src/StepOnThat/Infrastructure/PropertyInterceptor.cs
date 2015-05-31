using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DynamicProxy;

namespace StepOnThat.Infrastructure
{
    public class PropertyInterceptor : IInterceptor
    {
        private const string PropertyMatcher = @"(\s*\$\{[A-z0-9.-]+\}\s*)|(\s*\{\{[A-z0-9.-]+\}\}\s*)";
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
                    var name = Regex.Replace(invocation.ReturnValue as string, @"\$|{|}", "");
                    string variableValue;
                    try
                    {
                        variableValue = properties[name];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ApplicationException(String.Format("Could not find variable with name {0}", name));
                    }
                    invocation.ReturnValue = variableValue ?? "";
                }
            }
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