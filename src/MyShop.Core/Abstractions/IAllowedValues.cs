namespace MyShop.Core.Abstractions;
public interface IAllowedValues
{
    static abstract IReadOnlyCollection<object> AllowedValues { get; }
}
