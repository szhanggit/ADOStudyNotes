using FluentValidation;
using Services.Command.ImageMedia;
using System.IO;

namespace Services.Validators.Media
{
    public class RenameImageMediaCommandValidator : AbstractValidator<RenameImageMediaCommand>
    {
        public RenameImageMediaCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(p => p.MediaId)
                .NotEmpty()
                .NotNull();
            RuleFor(p => p.Keyword);
        }
    }
}