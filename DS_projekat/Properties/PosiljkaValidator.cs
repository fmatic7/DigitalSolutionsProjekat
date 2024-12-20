using FluentValidation;
using DS_projekat.Models;

namespace DS_projekat.Properties
{
    public class PosiljkaValidator : AbstractValidator<Posiljka>
    {
        public PosiljkaValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv ne sme biti prazan.")
                .WithMessage("Naziv ne sme biti duži od 100 karaktera.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status mora biti validna vrednost enuma.");

            RuleFor(x => x.DatumIsporuke)
            .GreaterThan(DateTime.UtcNow).WithMessage("Datum isporuke mora biti u budućnosti.")
            .When(x => x.DatumIsporuke.HasValue);

            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id mora biti validan.")
            .Must(id => id != Guid.Empty).WithMessage("Id mora biti validan GUID.")
            .Must(ValidGuid).WithMessage("Id mora biti validan GUID.");
        }

        private bool ValidGuid(Guid id)
        {
            return id != Guid.Empty;
        }
    }
}
