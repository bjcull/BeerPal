using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BeerPal.Web.Extensions
{
    public static class EnumHelper<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetDisplayValues()
        {
            return Enum.GetNames(typeof(T)).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null)
            {
                return string.Empty;
            }

            return descriptionAttributes.Length > 0 
                ? descriptionAttributes[0].Name 
                : value.ToString();
        }

        public static T GetValueFromName(string name)
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                var attribute = field.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }

            return default(T);
        }
        
        public static Expression<Func<T, String>> CreateEnumToStringExpression()
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("T must be an Enum type");
            }

            var enumValues = (T[])Enum.GetValues(type);
            var enumDisplayValues = GetDisplayValues();

            var inner = (Expression)Expression.Constant("");

            var parameter = Expression.Parameter(typeof(T));

            for (int i = 0; i < enumValues.Length; i++)
            {
                inner = Expression.Condition(
                    Expression.Equal(parameter, Expression.Constant(enumValues[i])),
                    Expression.Constant(enumDisplayValues[i]),
                    inner);
            }

            var expression = Expression.Lambda<Func<T, String>>(inner, parameter);

            return expression;
        }
    }

    public static class EnumExtensions
    {        
        public static string GetDisplayName(this Enum enumValue)
        {
            var value = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault();

            if (value == null)
                return string.Empty;

            return value.GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
    }
}