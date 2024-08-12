using System;
using System.Linq;
using System.Reflection;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;

namespace CUE4Parse.UE4.Assets.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class StructFallback : Attribute { }

    public static class StructFallbackUtil
    {
        public static ObjectMapper? ObjectMapper = new DefaultObjectMapper();

        public static object? MapToClass(this FStructFallback? fallback, Type type)
        {
            if (fallback == null)
            {
                return null;
            }

            object value;
            var dataConstructor = type.GetConstructor(new[] { typeof(FStructFallback) });
            if (dataConstructor != null)
            {
                // Let the constructor with FStructFallback assign the data
                value = dataConstructor.Invoke(new object[] { fallback });
            }
            else
            {
                // Or automatically map the values using reflection
                value = Activator.CreateInstance(type);
                ObjectMapper?.Map(fallback, value);
            }
            return value;
        }
    }

    public abstract class ObjectMapper
    {
        public abstract void Map(IPropertyHolder src, object dst);
    }

    public class DefaultObjectMapper : ObjectMapper
    {
        public override void Map(IPropertyHolder src, object dst)
        {
            var fields = dst.GetType().GetFields();
            foreach (var field in fields)
            {
                var propertyAttribute = field.GetCustomAttribute<UPropertyAttribute>();
                if (propertyAttribute is null) continue;

                var targetName = propertyAttribute.PropertyName ?? field.Name;

                var targetProperty = src.Properties.FirstOrDefault(prop => prop.Name.Text.Equals(targetName));
                if (targetProperty is null) continue;
            
                field.SetValue(dst, targetProperty.Tag?.GetValue(field.FieldType));
            }
        }
    }
}