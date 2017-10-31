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

        private static FieldInfo GetField(T instance, string fieldName)
        {
            FieldInfo field = null;
            Type type = instance.GetType();

            while (field == null && type != null)
            {
                field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
            }

            if (field == null)
            {
                throw new ArgumentException("field");
            }

            return field;
        }

        public static void SetFieldValue(T instance, string fieldName, object value)
        {
            GetField(instance, fieldName).SetValue(instance, value);
        }

        public static TValue GetFieldValue<TValue>(T instance, string fieldName)
        {
            return (TValue)GetField(instance, fieldName).GetValue(instance);
        }

        public static PropertyInfo GetProperty(Expression<Func<T>> property)
        {
            var memberExpression = property.Body as MemberExpression;
            var propertyInfo = memberExpression != null ? memberExpression.Member as PropertyInfo : null;

            if (propertyInfo == null)
            {
                throw new ArgumentException("property");
            }


            return propertyInfo;
        }

        private static PropertyInfo GetProperty(T instance, string propertyName)
        {
            PropertyInfo property = null;
            Type type = instance.GetType();

            while (property == null && type != null)
            {
                property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
            }

            return property;
        }

        public static void SetPropertyValue(T instance, string propertyName, object value)
        {
            GetProperty(instance, propertyName).SetValue(instance, value);
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