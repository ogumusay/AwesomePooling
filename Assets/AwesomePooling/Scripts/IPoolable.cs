namespace AwesomePooling
{
    public interface IPoolable
    {
        bool IsInUse { get; set; }
        void OnPooled();
        void OnSelected();
    }
}