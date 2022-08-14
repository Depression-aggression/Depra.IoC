namespace Depra.IoC.Tests
{
    internal class Service : IServiceTest
    {
    }

    internal class ServiceWithEmptyConstructor : IServiceTest
    {
        public ServiceWithEmptyConstructor()
        {
        }
    }

    internal class ServiceWithConstructor : IServiceTest
    {
        private readonly object _obj;
        private readonly string _message;
        
        public ServiceWithConstructor(object @obj, string message)
        {
            _obj = obj;
            _message = message;
        }
    }
}