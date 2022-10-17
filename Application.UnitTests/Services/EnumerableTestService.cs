using System;
using System.Collections;
using System.Collections.Generic;

namespace Depra.IoC.Application.UnitTests.Services;

internal class EnumerableTestService : ITestService, IEnumerable<EmptyGeneric>
{
    public IEnumerator<EmptyGeneric> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}