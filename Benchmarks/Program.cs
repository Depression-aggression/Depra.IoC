// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Validators;
using Depra.IoC.Benchmarks;

var benchmark = BenchmarkSwitcher.FromTypes([
	typeof(ContainerBenchmark)
]);

IConfig configuration = DefaultConfig.Instance
	.AddJob(Job.Default.WithToolchain(InProcessEmitToolchain.Instance))
	.AddValidator(JitOptimizationsValidator.FailOnError)
	.AddDiagnoser(MemoryDiagnoser.Default)
	.WithOptions(ConfigOptions.DisableOptimizationsValidator)
	.WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));

if (args.Length > 0)
{
	benchmark.Run(args, configuration);
}
else
{
	benchmark.RunAll(configuration);
}