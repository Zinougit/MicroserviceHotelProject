using CQRS.Core.Command;
using CQRS.Core.Infrastructure;

namespace Room.CMD.API.CommandDispatcher;

public class CommandDispatcher : ICommandDispatcher
{
    private Dictionary<Type,Func<BaseCommand,Task>> _Handlers = new();
    public void Register<T>(Func<T, Task> handler) where T: BaseCommand
    {
        if(_Handlers.ContainsKey(typeof(T))) {
            throw new IndexOutOfRangeException("This Hanlder is alredy registred");
        }
        _Handlers.Add(typeof(T),u=>handler((T)u));
    }

    public async Task Send(BaseCommand command)
    {
        if(_Handlers.TryGetValue(command.GetType(), out Func<BaseCommand,Task> Handler)){
           await Handler.Invoke(command);
        }else{
            throw new ArgumentNullException(nameof(_Handlers),"No Command Handler was Register");
        }
    }
}