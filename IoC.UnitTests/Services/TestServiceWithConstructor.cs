using System;

namespace Depra.IoC.UnitTests.Services;

internal sealed class TestServiceWithConstructor : ITestService
{
    public sealed class Token
    {
        private readonly Guid _guid;

        public Token() => _guid = Guid.NewGuid();

        public override string ToString() => _guid.ToString();
    }

    public TestServiceWithConstructor(Token token)
    {
        if (token == null)
        {
            throw new NullReferenceException();
        }
    }
}