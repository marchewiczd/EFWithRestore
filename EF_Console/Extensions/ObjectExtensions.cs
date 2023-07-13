using System.Reflection;

namespace EF_Console.Extensions
{
    public static class ObjectExtensions
    {
        public static void CopyValues(this object source, object destination)
        {
            if (destination == null)
                throw new Exception("Destination object cannot be null");

            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate)
                          && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };

            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
