using System.Threading.Tasks;

namespace CSLabs.Console
{
    public abstract class AsyncCommand<T>
    {
        public abstract Task Run(T options);
    }
}