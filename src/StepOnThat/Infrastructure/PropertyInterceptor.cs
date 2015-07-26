using System;
using System.Collections.Generic;
using System.Linq;
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
            if (InterceptSettersOfVariables(invocation))
                return;

            invocation.Proceed();

            InterceptGettersWithVariables(invocation);
        }

        private void InterceptGettersWithVariables(IInvocation invocation)
        {
            if (IsGetterProperty(invocation.Method)
                && IsPublicStringReturnType(invocation.Method)
                && ReturnsAVariable(invocation.ReturnValue))
            {
                if (invocation.ReturnValue != null)
                {
                    var originalValue = invocation.ReturnValue as string;
                    var newValue = ApplyPropertiesToValue(originalValue);
                    invocation.ReturnValue = newValue;
                }
            }
        }

        private bool InterceptSettersOfVariables(IInvocation invocation)
        {
            if (IsSetterProperty(invocation.Method) 
                && HasOneStringArgument(invocation.Method)
                && invocation.Arguments.Count() == 1
                && !ReturnsAVariable(invocation.Arguments.Single()) 
                && IsVoidReturnType(invocation.Method))
            {
                var corespondingGetter = FindGetterFromSetter(invocation);
                var getterResult = corespondingGetter.Invoke(invocation.InvocationTarget, new object[0]);
                if (getterResult != null && ReturnsAVariable(getterResult))
                {
                    var variableName = Regex.Replace(getterResult.ToString(), @"\$|{|}", "");
                    var setterValue = invocation.Arguments.Single().ToString();
                    try
                    {
                        properties[variableName] = setterValue;
                        return true;
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ApplicationException(String.Format("Could not find variable with name {0}",
                            variableName));
                    }
                }
            }
            return false;
        }

        private MethodInfo FindGetterFromSetter(IInvocation invocation)
        {
            return invocation.Method.DeclaringType
                .GetProperties()
                .First(prop => prop.GetSetMethod() == invocation.Method)
                .GetGetMethod();
        }

        public string ApplyPropertiesToValue(string originalValue)
        {
            var newValue = Regex.Replace(originalValue, PropertyMatcher, match =>
            {
                var name = Regex.Replace(match.Value, @"\$|{|}", "");

                if (properties.Contains(name))
                {
                    return properties[name];
                }
                else
                {
                    return match.Value;
                }
            });

            return newValue;
        }

        private static bool ReturnsAVariable(object value)
        {
            var result = value as string;

            return !string.IsNullOrEmpty(result) && Regex.IsMatch(result, PropertyMatcher);
        }

        private static bool IsGetterProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }

        private static bool IsSetterProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("set_");
        }

        private static bool IsPublicStringReturnType(MethodInfo method)
        {
            return method.IsPublic && (method.ReturnType == typeof (string));
        }

        private static bool IsVoidReturnType(MethodInfo method)
        {
            return method.IsPublic && (method.ReturnType == typeof (void));
        }

        private static bool HasOneStringArgument(MethodInfo method)
        {
            var parameters = method.GetParameters();
            return parameters.Count() == 1 && parameters.First().ParameterType == typeof (string);
        }
    }
}