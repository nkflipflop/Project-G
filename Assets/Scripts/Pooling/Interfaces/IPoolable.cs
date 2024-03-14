namespace Pooling.Interfaces
{
	public interface IPoolable
	{
		ObjectType Type { get; set; }
		int InitialSize { get; set; }
		
		void Initialize(System.Action<IPoolable> returnAction);
		void ReturnToPool();
	}
}