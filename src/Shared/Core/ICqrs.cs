namespace Shared.Core.Cqrs;

// Base interfaces for CQRS pattern
public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

public interface ICommand
{
}

public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

// Query dispatcher for dependency injection
public interface IQueryDispatcher
{
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query);
}

// Command dispatcher for dependency injection  
public interface ICommandDispatcher
{
    Task DispatchAsync(ICommand command);
    Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command);
} 