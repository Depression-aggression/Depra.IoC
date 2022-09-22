using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Validators;
using Depra.IoC.Application.Containers.Builders.Extensions;
using Depra.IoC.Application.Containers.Builders.Interfaces;
using Depra.IoC.Containers.Builders.Impl;
using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Scope;
using Depra.IoC.Scope;

namespace Depra.IoC.Benchmarks
{
    [InProcess]
    public class ContainerBenchmark
    {
        public interface IService
        {
        }

        private class Service : IService
        {
        }

        public class Controller
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
        public Controller Create() => new Controller(new Service());

        [Benchmark]
        public Controller Lambda() => _lambdaBased.Resolve<Controller>();

        [Benchmark]
        public Controller Reflection() => _reflectionBased.Resolve<Controller>();

        [GlobalSetup]
        public void GlobalSetup()
        {
            var lambdaBasedContainer = BuildContainer(new LambdaBasedContainerBuilder());
            var reflectionBasedContainer = BuildContainer(new ReflectionBasedContainerBuilder());

            _lambdaBased = lambdaBasedContainer.CreateScope();
            _reflectionBased = reflectionBasedContainer.CreateScope();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _lambdaBased.Dispose();
            _reflectionBased.Dispose();
        }
        
        private static IContainer BuildContainer(IContainerBuilder builder)
        {
            builder.RegisterTransient<IService, Service>()
                .RegisterTransient<Controller>();

            return builder.Build();
        }
    }
}