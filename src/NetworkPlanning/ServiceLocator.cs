using Unity;

namespace NetworkPlanning
{
    internal static class ServiceLocator
    {
        public static IUnityContainer Container { get; } = new UnityContainer();
    }
}
