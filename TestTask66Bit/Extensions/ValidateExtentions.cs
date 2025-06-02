using FluentValidation;
using System.Text.RegularExpressions;

namespace TestTask66Bit.Extensions
{
    public static partial class ValidateExtentions
    {
        public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            var regex = PhoneRegex();
            return ruleBuilder.Matches(regex);
        }

        //[GeneratedRegex(@"^(?:\+?7|8)?(?:[\s\-(_]+)?(\d{3})(?:[\s\-_)]+)?(\d{3})(?:[\s\-_]+)?(\d{2})(?:[\s\-_]+)?(\d{2})$")]
        [GeneratedRegex(@"^(\+7)((\d{10})|(\s\(\d{3}\)\s\d{3}\s\d{2}\s\d{2}))$")]
        private static partial Regex PhoneRegex();
    }
}
