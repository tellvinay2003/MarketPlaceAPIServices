using System;
using System.ComponentModel;
using System.Reflection;

namespace MarketPlaceService.DAL.Utilities
{
    public static  class EnumExtenstions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }


 public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttribute<TAttribute>();
    }

    

    }
}
