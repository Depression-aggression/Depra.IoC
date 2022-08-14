using Depra.IoC.Containers.Builders.Impl;
using Depra.IoC.Tests.Abstract;
using NUnit.Framework;

namespace Depra.IoC.Tests
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