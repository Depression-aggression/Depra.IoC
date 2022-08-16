using System;
using System.Collections;
using System.Collections.Generic;

namespace Depra.IoC.Tests
{
    internal interface IServiceTest
    {
    }

    internal class Service : IServiceTest
    {
        public bool IsCreated => true;
    }

    internal class ServiceWithEmptyConstructor : IServiceTest
    {
        public ServiceWithEmptyConstructor()
        {
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
        
        public ServiceWithConstructor(Token token)
        {
            if (token == null)
            {
                throw new NullReferenceException();
            }
        }
    }

    internal class EmptyGeneric
    {
    }

    internal class GenericService<T> : IServiceTest where T : EmptyGeneric
    {
        public GenericService(T value)
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }
        }
    }
    
    internal class EnumerableService : IServiceTest, IEnumerable<EmptyGeneric>
    {
        public IEnumerator<EmptyGeneric> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}