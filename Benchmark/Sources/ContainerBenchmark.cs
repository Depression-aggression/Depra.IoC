using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Validators;
using Depra.IoC.Containers.Builders.Extensions;
using Depra.IoC.Containers.Builders.Impl;
using Depra.IoC.Containers.Builders.Interfaces;
using Depra.IoC.Scope;

namespace Depra.IoC.Benchmark
{
    [InProcess]
    [MemoryDiagnoser]
    [Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ContainerBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                WithOptions(ConfigOptions.Default)
                    .AddValidator(JitOptimizationsValidator.FailOnError)
                    .AddLogger(ConsoleLogger.Default)
                    .AddColumnProvider(DefaultColumnProviders.Instance);
            }
        }

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
            var lambdaBasedBuilder = new LambdaBasedContainerBuilder();
            var reflectionBasedBuilder = new ReflectionBasedContainerBuilder();

            InitContainer(lambdaBasedBuilder);
            InitContainer(reflectionBasedBuilder);

            _lambdaBased = lambdaBasedBuilder.Build().CreateScope();
            _reflectionBased = reflectionBasedBuilder.Build().CreateScope();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _lambdaBased.Dispose();
            _reflectionBased.Dispose();
        }
        
        private static void InitContainer(IContainerBuilder builder)
        {
            builder.RegisterTransient<IService, Service>()
                .RegisterTransient<Controller>();
        }
    }
}