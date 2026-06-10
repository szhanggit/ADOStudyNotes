using FluentValidation;
using Services.Command.ImageMedia;

namespace Services.Validators.Media
{
    public class CreateImageMediaCommandValidator : AbstractValidator<CreateImageMediaCommand>
    {
        public CreateImageMediaCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Type)
                .NotEmpty()
                .NotNull()
                .IsInEnum();

            RuleFor(p => p.Image)
                .NotEmpty()
                .NotNull()
                .Must(p => p.Length <= 1000000).WithMessage("Max file size is at least 1 MB");
            //TX2 allows any type
            //.Must(p => p.ContentType == "image/jpeg" 
            //        || p.ContentType == "image/png" 
            //        || p.ContentType == "image/bmp"
            //        || p.ContentType == "image/jpg")
            //.WithMessage("Invalid image type");
        }
    }
}
