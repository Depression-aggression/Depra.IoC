// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Activation;
using Depra.IoC.Builder;
using Depra.IoC.Scope;

namespace Depra.IoC.Benchmarks;

public class ContainerBenchmark
{
	private IScope _lambdaBased;
	private IScope _reflectionBased;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_lambdaBased = BuildContainer(new LambdaBasedActivationBuilder())
			.CreateScope();
		_reflectionBased = BuildContainer(new ReflectionBasedActivationBuilder())
			.CreateScope();

		IContainer BuildContainer(IActivationBuilder activationBuilder) =>
			new ContainerBuilder(activationBuilder)
				.RegisterTransient<IService, Service>()
				.RegisterTransient<Controller>()
				.Build();
	}

	[GlobalCleanup]
	public void GlobalCleanup()
	{
		_lambdaBased.Dispose();
		_reflectionBased.Dispose();
	}

	[Benchmark(Baseline = true)]
	public IController CreateUsingConstructor() =>
		new Controller(new Service());

	[Benchmark]
	public IController ResolveByTypeUsingLambdas() =>
		_lambdaBased.Resolve<Controller>();

	[Benchmark]
	public IController ResolveByTypeUsingReflection() =>
		_reflectionBased.Resolve<Controller>();

	[Benchmark]
	public IController ResolveByInterfaceUsingLambdas() =>
		_lambdaBased.Resolve<Controller>();

	[Benchmark]
	public IController ResolveByInterfaceUsingReflection() =>
		_reflectionBased.Resolve<Controller>();

	private interface IService { }

	public interface IController { }

	private class Service : IService { }

	private class Controller : IController
	{
		private readonly IService _service;

		public Controller(IService service) => _service = service;
	}
}