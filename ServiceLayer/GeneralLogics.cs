using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class GeneralLogics
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public DateTime CurrentIndianTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        }
        public Guid CreateUniqueId()
        {
            return Guid.NewGuid();
        }
        public bool ContainsOnlyDigits(string testingString)
        {
            return Regex.IsMatch(testingString, @"^[0-9]+$");
        }

        public bool ContainsOnlyAlphabets(List<string> listOfString)
        {
            for(int i = 0; i < listOfString.Count; i++)
            {
                if (!Regex.IsMatch(listOfString[i], @"^[A-Za-z]+$"))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ContainsOnlyAlphabets(string testingString)
        {
            return Regex.IsMatch(testingString, @"^[A-Za-z]+$");
        }
        public bool ValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
        }
        
        public bool ContainsAnyNullorWhiteSpace(List<string> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(list[i]) || String.IsNullOrEmpty(list[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
