using System.Threading.Tasks;

namespace CSLabs.ConsoleUtil
{
    public abstract class AsyncCommand<T>
    {
        public abstract Task Run(T options);
    }
}