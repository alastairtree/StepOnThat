using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace StepOnThat
{
    internal class JsonTypePropertyConverter<TType> : CustomCreationConverter<TType> where TType : new()
    {
        public Type DefaultyValueType { get; set; }

        public override TType Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        private TType Create(JObject jObject)
        {
            var typeName = (string) jObject.Property("type");

            if (String.IsNullOrEmpty(typeName))
                typeName = DefaultyValueType == null ? "" : DefaultyValueType.Name;

            IDictionary<string, Type> lookup = GetTypeNameLookup<TType>(true);

            Type matchedType;
            if (lookup.TryGetValue(typeName, out matchedType))
                return (TType) Activator.CreateInstance(matchedType);

            if (string.IsNullOrEmpty(typeName))
                throw new ApplicationException(
                    "Cannot deserialise JSON becasue of missing or empty type property. Use {type='SomeName'} instead.");

            throw new ApplicationException(String.Format("The given type {0} is not supported!", typeName));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            TType target = Create(jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        private IDictionary<string, Type> GetTypeNameLookup<T>(bool ignoreCase = false)
        {
            StringComparer opts = (ignoreCase
                ? StringComparer.InvariantCultureIgnoreCase
                : StringComparer.InvariantCulture);
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => typeof (T).IsAssignableFrom(type))
                .ToDictionary(type => type.Name, type => type, opts);
        }
    }
}