using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HireMe.Entities.Enums
{
    public enum ExprienceLevels : int
    {
        [Display(Name = "Начинаещ (1 - 3 г.)")]
        Beginner = 1,

        [Display(Name = "Средно ниво (3 - 5 г.)")]
        Intermediate = 2,

        [Display(Name = "Експерт (5+ г.)")]
        Expert = 3
    }

    public static class EnumExtensions 
    {
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }
        public static string GetPrompt(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Prompt : enu.ToString();
        }
        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        public static string GetShortName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.ShortName : enu.ToString();
        }
        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }

}