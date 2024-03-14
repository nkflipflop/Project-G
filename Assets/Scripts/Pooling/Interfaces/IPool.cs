namespace Pooling.Interfaces
{
    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }
}