using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CSLabs.Api.Util
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EitherRequired : ValidationAttribute
    {
        private string[] propNames;
        

        public EitherRequired(params string[] propNames)
        {
            this.propNames = propNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] allDisplayNames = getDisplayNames(validationContext.DisplayName);
            bool anyNotNull = value != null;

            foreach (var propName in this.propNames)
            {
                var prop = validationContext.ObjectInstance.GetType().GetProperty(propName);
                if (prop != null)
                {
                    if (prop.GetValue(validationContext.ObjectInstance) != null)
                        anyNotNull = true;
                }
            }

            if (!anyNotNull)
                return new ValidationResult(GetErrorMessage(allDisplayNames));
            return ValidationResult.Success;
        }

        private string[] getDisplayNames(string currentPropName)
        {
            string[] allDisplayNames = new string[this.propNames.Length + 1];
            allDisplayNames[0] = getSentence(currentPropName);
            for (int i = 0; i < this.propNames.Length; i++)
                allDisplayNames[i + 1] = getSentence(this.propNames[i]);
            Array.Sort(allDisplayNames, (x,y) => String.Compare(x, y));
            return allDisplayNames;
        }

        private string getSentence(string propName)
        {
            string spacedString = Regex.Replace(propName, @"\B([A-Z])", @" $1");
            return  spacedString.ToLower();
        }

        public string GetErrorMessage(string[] allPropNames)
        {
            var phrase = string.Join(" or ", allPropNames);
            return $"Either {phrase} is required.";
        }
        
    }
}