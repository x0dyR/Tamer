using System;
using System.Collections.Generic;

namespace Tamer.Develop.DI
{
    public class DiContainer : IDisposable
    {
        private readonly Dictionary<Type, Registration> _container = new();

        private readonly DiContainer _parent;

        private readonly List<Type> _requests = new();

        public DiContainer() : this(null) { }

        public DiContainer(DiContainer parent) => _parent = parent;

        public Registration RegisterAsSingle<T>(Func<DiContainer, T> creator)
        {
            if (IsAlreadyRegistered<T>())
                throw new InvalidOperationException($"Container already contains a registration {typeof(T)}");

            Registration registration = new Registration(container => creator.Invoke(container));
            _container[typeof(T)] = registration;
            return registration;
        }

        public T Resolve<T>()
        {
            if (_requests.Contains(typeof(T)))
                throw new InvalidOperationException($"Cycle resolve for {typeof(T)}");

            _requests.Add(typeof(T));

            try
            {
                if (_container.TryGetValue(typeof(T), out Registration registration))
                    return CreateFrom<T>(registration);

                if (_parent != null)
                    return _parent.Resolve<T>();
            }
            finally
            {
                _requests.Remove(typeof(T));
            }

            throw new InvalidOperationException($"No registration for {typeof(T)}");
        }

        public void Initialize()
        {
            foreach (Registration registration in _container.Values)
            {
                if (registration.Instance == null && registration.IsNonLazy)
                    registration.Instance = registration.Creator(this);

                if (registration.Instance is IInitializable initializable)
                    initializable.Initialize();
            }
        }

        public void Dispose()
        {
            foreach (Registration registration in _container.Values)
                if (registration.Instance is IDisposable disposable)
                    disposable.Dispose();
        }

        private T CreateFrom<T>(Registration registration)
        {
            if (registration.Instance == null && registration.Creator != null)
                registration.Instance =
                    registration
                        .Creator(this); //registration.Creator?.Invoke(this) одно и тоже, просто выше мы уже проверили на null

            return (T)registration.Instance;
        }

        private bool IsAlreadyRegistered<T>()
            => _container.ContainsKey(typeof(T)) || _parent != null && _parent.IsAlreadyRegistered<T>();

        public class Registration
        {
            public Func<DiContainer, object> Creator { get; }

            public object Instance { get; set; }

            public bool IsNonLazy { get; private set; }

            public Registration(object instance) => Instance = instance;

            public Registration(Func<DiContainer, object> creator) => Creator = creator;

            public void NonLazy() => IsNonLazy = true;
        }
    }

}