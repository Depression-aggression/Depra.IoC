using System;
using System.Collections;
using System.Collections.Generic;
using Depra.IoC.UnitTests.Services;

namespace Depra.IoC.UnitTests.Services;

internal sealed class EnumerableTestService : ITestService, IEnumerable<EmptyGeneric>
{
    public IEnumerator<EmptyGeneric> GetEnumerator() =>
        throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}