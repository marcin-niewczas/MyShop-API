using MyShop.Application.Abstractions;

namespace MyShop.Application.Commands;
public interface ICommand : IMessage
{
}

public interface ICommand<in TResult> : ICommand
{
}
