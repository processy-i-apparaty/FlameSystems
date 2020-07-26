namespace FlameSystems.Infrastructure.ActionFire
{
    public class ActionFireReturn<T>
    {
        public ActionFireReturn(T obj, bool result)
        {
            Object = obj;
            Result = result;
        }

        public T Object { get; set; }
        public bool Result { get; set; }
    }
}