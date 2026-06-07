namespace Catalog.Products.Features.GetProductByName;

public record GetProductByNameQuery(string Name)
    : IQuery<GetProductByNameResult>;
public record GetProductByNameResult(IEnumerable<ProductDto> Products);

internal class GetProductByNameHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByNameQuery, GetProductByNameResult>
{
    public async Task<GetProductByNameResult> Handle(GetProductByNameQuery query, CancellationToken cancellationToken)
    {
        // get products by Name using dbContext
        // return result

        var products = await dbContext.Products
                .AsNoTracking()
                .Where(p => p.Name.Contains(query.Name))
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

        //mapping product entity to productdto
        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductByNameResult(productDtos);
    }
}
