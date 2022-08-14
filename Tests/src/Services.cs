using System;

namespace Depra.IoC.Tests
{
    internal interface IServiceTest
    {
        bool IsCreated { get; }
    }

    internal class Service : IServiceTest
    {
        public bool IsCreated => true;
    }

    internal class ServiceWithEmptyConstructor : IServiceTest
    {
        public bool IsCreated { get; }

        public ServiceWithEmptyConstructor()
        {
            IsCreated = true;
        }
    }

    internal class ServiceWithConstructor : IServiceTest
    {
        public class Token
        {
            private Guid _guid;
            
            public Token()
            {
                _guid = Guid.NewGuid();
            }

            public override string ToString() => _guid.ToString();
        }

        private readonly Token _token;

        public bool IsCreated => _token != null;

        public ServiceWithConstructor(Token token)
        {
            _token = token;
        }
    }
}