namespace ShaRide.Application.Executables.Abstraction
{
    /// <summary>
    /// Base of executable classes.
    /// </summary>
    public abstract class ExecutableBase
    {
        public abstract void Execute();
    }

    public abstract class ExecutableBase<T> : ExecutableBase
    {
        public abstract T Execute2(T model);

        public sealed override void Execute()
        {
            
        }
    }
}