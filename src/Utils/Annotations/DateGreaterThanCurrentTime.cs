
using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoAppApi.Utils.Annotations
{
	public class DateGreaterThanCurrentTime: ValidationAttribute
	{
		public override string FormatErrorMessage(string name)
		{
			return "Date value should not in the past";
		}

		protected override ValidationResult IsValid(object objValue,
													   ValidationContext validationContext)
		{
			var dateValue = objValue as DateTime? ?? new DateTime();

			if (dateValue.Date <= DateTime.Now.Date)
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}
			return ValidationResult.Success;
		}
	}
}

