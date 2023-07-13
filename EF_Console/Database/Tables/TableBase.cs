using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EF_Console.Database.Tables
{
    public abstract class TableBase : ICloneable
    {
        public virtual object? GetKey() =>
            GetType().GetProperties()?
                .Select(pi => new { Property = pi, Attribute = pi.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() as KeyAttribute })?
                .FirstOrDefault(x => x.Attribute != null)?
                .Property.GetValue(this, null);

        public object Clone() => MemberwiseClone();
    }
}