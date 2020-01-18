using System.Threading.Tasks;

namespace CSLabsConsole
{
    public abstract class AsyncCommand<T>
    {
        public abstract Task Run(T options);
    }
}