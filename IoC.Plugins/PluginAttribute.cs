// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.IoC.Plugins;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class PluginAttribute : Attribute { }