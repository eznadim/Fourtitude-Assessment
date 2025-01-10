using FluentValidation;

namespace TransactionAPI.Models.Validation;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.PartnerKey).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PartnerRefNo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PartnerPassword).NotEmpty().MaximumLength(50);
        RuleFor(x => x.TotalAmount).GreaterThan(0);
        RuleFor(x => x.Timestamp).NotEmpty()
            .Must(BeValidIsoDateTime).WithMessage("Invalid timestamp format");
        RuleFor(x => x.Sig).NotEmpty();

        RuleForEach(x => x.Items).SetValidator(new ItemDetailValidator());
    }

    private bool BeValidIsoDateTime(string timestamp)
    {
        return DateTime.TryParse(timestamp, out _);
    }
}

public class ItemDetailValidator : AbstractValidator<ItemDetail>
{
    public ItemDetailValidator()
    {
        RuleFor(x => x.PartnerItemRef).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Qty).InclusiveBetween(1, 5);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}