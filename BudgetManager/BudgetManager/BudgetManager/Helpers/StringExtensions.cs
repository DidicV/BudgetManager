using System;
using System.Linq;

namespace BudgetManager.Helpers
{
    public static class StringExtensions
    {
        public static bool IsOneOf(this string value, params string[] options)
        {
            return options.Any(option => value.Equals(option, StringComparison.OrdinalIgnoreCase));
        }
    }
}