using FluentValidation;
using System.Data;

namespace WebNaN.Models.Validator
{
    public class AddValidator : AbstractValidator<Product>

    {

        public AddValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Məhsulun adı olmalıdır");
            RuleFor(x => x.ProductCategoryId).NotNull().NotEmpty().WithMessage("Ketogori bos olmamalidir");
            RuleFor(x => x.ProductInfo).NotEmpty().WithMessage("Məhsul Haqqında Məlumat olmalıdır");

        }


    }
}
