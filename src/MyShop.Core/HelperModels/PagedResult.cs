using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.HelperModels;
public sealed record PagedResult<TModel>(
    IReadOnlyCollection<TModel> Data,
    int TotalCount
    ) where TModel : class, IModel;
