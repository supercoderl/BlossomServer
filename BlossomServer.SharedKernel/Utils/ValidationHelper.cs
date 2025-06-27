using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlossomServer.SharedKernel.Utils
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Pattern chuẩn RFC 5322 đơn giản
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(input);
        }
    }
}
