using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SandTigerShark.GameServer.Services.Utils
{
    public static class Reflect<T>
    {
        public static string PropertyName<TValue>(Expression<Func<T, TValue>> property)
        {
            var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException("property");

            return propertyInfo.Name;
        }

        public static string MethodName<TValue>(Expression<Func<T, TValue>> property)
        {
            return (property.Body as MethodCallExpression).Method.Name;
        }

        public static PropertyInfo[] GetProperties(Func<PropertyInfo, bool> predicate = null)
        {
            return typeof(T).GetProperties()
                            .Where(p => predicate == null || predicate(p))
                            .ToArray();
        }
    }

    public static class Reflect
    {
        public static string[] GetEnumNames(Type enumType)
        {
            return Enum.GetNames(enumType);
        }
    }
}