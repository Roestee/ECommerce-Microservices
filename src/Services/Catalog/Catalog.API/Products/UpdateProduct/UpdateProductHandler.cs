﻿namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id
    , string Name
    , List<string> Category
    , string Description
    , string ImageFile
    , decimal Price) : ICommand<UpdateProductCommandResult>;

public sealed record UpdateProductCommandResult(bool IsSuccess);

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
