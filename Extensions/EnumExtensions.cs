using System.ComponentModel;

namespace Abstractions.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value is null) return string.Empty;

        var attribute = value.GetAttribute<DescriptionAttribute>();

        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static string[] ToArray(this Enum value)
        => value.ToString().Split(", ");

    private static T? GetAttribute<T>(this Enum value) where T : Attribute
    {
        if (value is null) return null;

        var member = value.GetType().GetMember(value.ToString());

        var attributes = member[0].GetCustomAttributes(typeof(T), false);

        return (T)attributes[0];
    }

    /// <summary> Creates a KeyValuePair from the given enum /// </summary>
    public static List<KeyValuePair<string, int>> GetEnumList<T>()
    {
        var list = new List<KeyValuePair<string, int>>();

        foreach (var e in Enum.GetValues(typeof(T)))
            list.Add(new KeyValuePair<string, int>(e?.ToString() ?? string.Empty, (int?)e ?? 0));

        return list;
    }
}
