using Depra.IoC.Containers.Builders.Impl;
using NUnit.Framework;

namespace IoC.Tests
{
    public class ReflectionBasedContainerTests : BaseContainerTests
    {
        [SetUp]
        public override void SetUp()
        {
            Builder = new ReflectionBasedContainerBuilder();
        }
    }
}