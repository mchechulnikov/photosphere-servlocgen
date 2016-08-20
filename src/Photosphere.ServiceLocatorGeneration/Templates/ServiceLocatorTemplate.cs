namespace Photosphere.ServiceLocatorGeneration.Templates
{
    internal static class ServiceLocatorTemplate
    {
        public static string ServiceLocator(string usingDirectives, string classPrefix, string ctorConfig, string ctorBody)
            => string.Format(TemplatesResource.ServiceLocator, usingDirectives, classPrefix, ctorConfig, ctorBody);

        public static string UsingDirective(string ns)
            => string.Format(TemplatesResource.UsingDirective, ns);

        public static string VariableStatement(string variableName, string assignment)
            => string.Format(TemplatesResource.VariableStatement, variableName, assignment);

        public static string NewInstanceStatement(string className, string parameters)
            => string.Format(TemplatesResource.NewInstanceStatement, className, parameters);

        public static string AddToDictionaryStatement(string dictionaryName, string key, string value)
            => string.Format(TemplatesResource.AddToDictinaryStatement, dictionaryName, key, value);

        public static string TypeofExpression(string argument)
            => string.Format(TemplatesResource.TypeofExpression, argument);
    }
}