namespace Balls
{
    public interface IPoolableObject : IResettableNonStaticData
    {
        void BeforeReset();
        void AfterReset();
    }
}
