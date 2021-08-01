namespace CSLabs.ConsoleUtil
{
    public abstract class Command<T>
    {
        public abstract void Run(T options);
    }
}