namespace MyShop.Application.Queries;
public interface IQuery
{
}

public interface IQuery<in TResult> : IQuery
{
}
