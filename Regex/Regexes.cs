namespace Abstractions.Regex;

public static class Regexes
{
    //Date regex requires format dd/MM/YYYY
    public const string Date = @"^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))) ?((20|21|22|23|[01]\d|\d)(([:.][0-5]\d){1,2}))?$";

    public const string Decimal = @"^((-?[1-9]+)|[0-9]+)(\.?|\,?)([0-9]*)$";

    //public const string Email = @"^([a-z0-9_\.\-]{3,})@([\da-z\.\-]{3,})\.([a-z\.]{2,6})$";

    public const string Email = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$";

    public const string Hex = "^#?([a-f0-9]{6}|[a-f0-9]{3})$";

    public const string Integer = "^((-?[1-9]+)|[0-9]+)$";

    public const string Login = "^[a-z0-9_-]{10,50}$";

    public const string Password = @"^.*(?=.{10,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&+=]).*$";

    public const string Tag = @"^<([a-z1-6]+)([^<]+)*(?:>(.*)<\/\1>| *\/>)$";

    public const string Time = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$";

    public const string Url = @"^((https?|ftp|file):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";

    public const string MobileNumberBasic = "^[0][0-9]*[0-9]*$"; //starts with 0 numeric digits only

    public const string MobileNumberLessStrict = @"^(?!0+$)(\+\d{1,3}[- ]?)?(?!0+$)\d{10}$"; // Complex rules with country code etc

    public const string MobileNumber = @"^((?:\+27|27)|0)(\d{2})-?(\d{3})-?(\d{4})$"; // Chips Mobile number validation

    public const string VatNumber = "^[4][0-9]*[0-9]*$"; //starts with 4 with numeric digits

    public const string Numeric = @"[0-9]$";
}
