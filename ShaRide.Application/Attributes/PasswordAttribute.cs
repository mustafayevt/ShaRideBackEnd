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
            var valueAsString = value.ToString() ?? string.Empty;
            
            List<bool> validations = new List<bool>
            {
                Regex.IsMatch(valueAsString, RegexConstraints.HasCharacters),
                Regex.IsMatch(valueAsString, RegexConstraints.HasNumbers),
                Regex.IsMatch(valueAsString, RegexConstraints.HasLowerCases),
                Regex.IsMatch(valueAsString, RegexConstraints.HasUpperCases)
            };


            return validations.Count(x => x) >= 3;
        }
    }
}