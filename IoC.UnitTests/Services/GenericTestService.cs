using System;
using Depra.IoC.UnitTests.Services;

namespace Depra.IoC.UnitTests.Services;

internal sealed class GenericTestService<T> : ITestService where T : EmptyGeneric
{
    public GenericTestService(T value)
    {
        if (value == null)
        {
            throw new NullReferenceException();
        }
    }
}