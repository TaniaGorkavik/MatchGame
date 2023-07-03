using Zenject;

namespace MatchGame.Utils
{
    public static class ContainerExtensions
    {
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindCustomClass<T>(this DiContainer container)
            where T : new()
        {
            var classObject = new T();
            return container.BindCustomClass<T>(classObject);
        }

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindCustomClass<T>(this DiContainer container,
            T classObject)
        {
            container.QueueForInject(classObject);
            return container.BindInterfacesAndSelfTo(typeof(T)).FromInstance(classObject);
        }
    }
}