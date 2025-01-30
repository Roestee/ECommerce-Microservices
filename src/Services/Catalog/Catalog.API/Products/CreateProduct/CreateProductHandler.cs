﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public sealed record CreateProductCommand(string Name
    , List<string> Category
    , string Description
    , string ImageFile
    , decimal Price): ICommand<CreateProductResult>;

public sealed record CreateProductResult(Guid Id);

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
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

        return new CreateProductResult(Guid.NewGuid());
    }
}
