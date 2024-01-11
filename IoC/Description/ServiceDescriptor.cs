// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Enums;

namespace Depra.IoC.Description
{
    public abstract class ServiceDescriptor
    {
        protected ServiceDescriptor(Type type, LifetimeType lifetime)
        {
            Type = type;
            Lifetime = lifetime;
        }

        public Type Type { get; }
        public LifetimeType Lifetime { get; }
    }
}