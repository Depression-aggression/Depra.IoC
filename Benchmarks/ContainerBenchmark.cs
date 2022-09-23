using BenchmarkDotNet.Attributes;
using Depra.IoC.Application.Activation;
using Depra.IoC.Application.Builder;
using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Extensions;
using Depra.IoC.Domain.Scope;
using Depra.IoC.Infrastructure.Activation;

namespace Depra.IoC.Benchmarks
{
    [MemoryDiagnoser]
    public class ContainerBenchmark
    {
        public interface IService { }

        private class Service : IService { }

        public interface IController
        {
            
        }
        
        public class Controller : IController
        {
            private readonly IService _service;

            public Controller(IService service)
            {
                _service = service;
            }
        }

        private IScope _lambdaBased;
        private IScope _reflectionBased;

        [Benchmark(Baseline = true)]
        public IController CreateUsingConstructor() => new Controller(new Service());

        [Benchmark]
        public IController ResolveByTypeUsingLambdas() => _lambdaBased.Resolve<Controller>();

        [Benchmark]
        public IController ResolveByTypeUsingReflection() => _reflectionBased.Resolve<Controller>();

        [Benchmark]
        public IController ResolveByInterfaceUsingLambdas() => _lambdaBased.Resolve<Controller>();

        [Benchmark]
        public IController ResolveByInterfaceUsingReflection() => _reflectionBased.Resolve<Controller>();

        [GlobalSetup]
        public void GlobalSetup()
        {
            var lambdaBasedContainer = BuildContainer(new LambdaBasedActivationBuilder());
            var reflectionBasedContainer = BuildContainer(new ReflectionBasedActivationBuilder());

            _lambdaBased = lambdaBasedContainer.CreateScope();
            _reflectionBased = reflectionBasedContainer.CreateScope();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _lambdaBased.Dispose();
            _reflectionBased.Dispose();
        }

        private static IContainer BuildContainer(IActivationBuilder activationBuilder)
        {
            var builder = new ContainerBuilder(activationBuilder)
                .RegisterTransient<IService, Service>()
                .RegisterTransient<Controller>();

            return builder.Build();
        }
    }
}