using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            List<bool> validations = new List<bool>
            {
                Regex.IsMatch(value.ToString(), RegexConstraints.HasCharacters),
                Regex.IsMatch(value.ToString(), RegexConstraints.HasNumbers),
                Regex.IsMatch(value.ToString(), RegexConstraints.HasLowerCases),
                Regex.IsMatch(value.ToString(), RegexConstraints.HasUpperCases)
            };


            return validations.Count(x => x) >= 3;
        }
    }
}