using System;

namespace Depra.IoC.Application.UnitTests.Services;

internal class GenericTestService<T> : ITestService where T : EmptyGeneric
{
    public GenericTestService(T value)
    {
        if (value == null)
        {
            throw new NullReferenceException();
        }
    }
}