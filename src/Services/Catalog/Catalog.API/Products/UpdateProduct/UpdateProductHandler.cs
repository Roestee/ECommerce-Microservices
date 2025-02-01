namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id
    , string Name
    , List<string> Category
    , string Description
    , string ImageFile
    , decimal Price) : ICommand<UpdateProductCommandResult>;

public sealed record UpdateProductCommandResult(bool IsSuccess);

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Id is required!");
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required!")
            .Length(3, 150).WithMessage("Name must be between 2 and 150 characters");
        RuleFor(p => p.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required!")
            .Length(3, 250).WithMessage("ImageFile must be between 3 and 250 characters");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required!");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0!");
    }
}

internal sealed class UpdateProductCommandHandler(IDocumentSession session
    , ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
{
    public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if(product is null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductCommandResult(true);
    }
}
