using System;

namespace Depra.IoC.Application.UnitTests.Services;

internal class TestServiceWithConstructor : ITestService
{
    public class Token
    {
        private Guid _guid;

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