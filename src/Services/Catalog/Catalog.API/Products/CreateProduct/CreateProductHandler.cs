namespace Catalog.API.Products.CreateProduct;

public sealed record CreateProductCommand(string Name
    , List<string> Category
    , string Description
    , string ImageFile
    , decimal Price): ICommand<CreateProductResult>;

public sealed record CreateProductResult(Guid Id);

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
                   .NotEmpty().WithMessage("Name is required!")
                   .Length(3, 150).WithMessage("Name must be between 3 and 150 characters");
        RuleFor(p => p.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required!")
            .Length(3, 250).WithMessage("ImageFile must be between 3 and 250 characters");
        RuleFor(p=>p.Category).NotEmpty().WithMessage("Category is required!");
        RuleFor(p=>p.Price).GreaterThan(0).WithMessage("Price must be greater than 0!");
    }
}

internal sealed class CreateProductCommandHandler(
    IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
