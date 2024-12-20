using DS_projekat.Models;
using FluentValidation;

namespace DS_projekat.Validators
{
    public class PosiljkaValidator : AbstractValidator<Posiljka>
    {
        public PosiljkaValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv ne sme biti prazan.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status mora biti validna vrednost enuma.");

            RuleFor(x => x.DatumIsporuke)
                .GreaterThan(DateTime.UtcNow).WithMessage("Datum isporuke mora biti u buducnosti.")
                .When(x => x.DatumIsporuke.HasValue);

            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id ne sme biti prazan.")
            .Must(id => id != Guid.Empty).WithMessage("Id mora biti validan GUID.");
        }
    }
}
