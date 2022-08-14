using BenchmarkDotNet.Running;

namespace Depra.IoC.Benchmark
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<ContainerBenchmark>();
        }
    }
}