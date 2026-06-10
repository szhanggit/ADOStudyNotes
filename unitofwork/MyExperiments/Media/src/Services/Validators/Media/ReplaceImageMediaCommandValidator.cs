using FluentValidation;
using Services.Command.ImageMedia;

namespace Services.Validators.Media
{
    public class ReplaceImageMediaCommandValidator : AbstractValidator<ReplaceImageMediaCommand>
    {
        public ReplaceImageMediaCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;


            RuleFor(p => p.MediaId)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.BlobName)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.Image)
                .NotEmpty()
                .NotNull()
                .Must(p => p.Length <= 1000000).WithMessage("Max file size is at least 1 MB")
                .Must(p => p.ContentType == "image/jpeg"
                        || p.ContentType == "image/png"
                        || p.ContentType == "image/bmp"
                        || p.ContentType == "image/jpg")
                .WithMessage("Invalid image type");
        }
    }
}
