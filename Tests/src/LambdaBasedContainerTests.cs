using Depra.IoC.Containers.Builders.Impl;
using NUnit.Framework;

namespace IoC.Tests
{
    public class LambdaBasedContainerTests : BaseContainerTests
    {
        [SetUp]
        public override void SetUp()
        {
            Builder = new LambdaBasedContainerBuilder();
        }
    }
}