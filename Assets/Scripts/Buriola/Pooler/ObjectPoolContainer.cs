namespace Buriola.Pooler
{
    public class ObjectPoolContainer<T>
    {
        public bool Used { get; private set; }

        public void Consume()
        {
            Used = true;
        }

        public T Item { get; set; }

        public void Release()
        {
            Used = false;
        }
    }
}
