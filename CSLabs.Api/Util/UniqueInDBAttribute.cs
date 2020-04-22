using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Models;

namespace CSLabs.Api.Util
{
    public class UniqueInDBAttribute : ValidationAttribute
    {
        public string ColumnName { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (IPrimaryKeyModel)validationContext.ObjectInstance;
            var propertyName = validationContext.MemberName;
            var propertyValue = model.GetType().GetProperty(propertyName).GetValue(model);
            return validationContext.ValidateUnique<IPrimaryKeyModel>(ColumnName, propertyValue, model.Id,
                propertyName + " already exists", null, model.GetType());
        }
    }
}