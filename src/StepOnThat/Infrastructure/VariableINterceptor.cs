using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DynamicProxy;

namespace StepOnThat.Infrastructure
{
    public class VariableInterceptor : IInterceptor
    {
        private const string variableMatcher = @"(\s*\$\{[A-z0-9.-]+\}\s*)|(\s*\{\{[A-z0-9.-]+\}\}\s*)";
        private readonly IVariables variables;

        public VariableInterceptor(IVariables variables)
        {
            this.variables = variables;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (IsGetterProperty(invocation.Method)
                && IsPublicStringReturnType(invocation.Method)
                && ReturnsAVariable(invocation))
            {
                var valueOfVariableWithThatName = variables[invocation.ReturnValue as string];

                invocation.ReturnValue = valueOfVariableWithThatName ?? "";
            }
        }

        private static bool ReturnsAVariable(IInvocation invocation)
        {
            var result = invocation.ReturnValue as string;

            return !string.IsNullOrEmpty(result) && Regex.IsMatch(result, variableMatcher);
        }

        private static bool IsGetterProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }

        private static bool IsPublicStringReturnType(MethodInfo method)
        {
            return method.IsPublic && method.ReturnType.IsValueType && (method.ReturnType == typeof (string));
        }
    }
}