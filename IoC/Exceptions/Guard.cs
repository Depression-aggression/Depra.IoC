// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Depra.IoC.Exceptions
{
	internal static class Guard
	{
		[Conditional(Conditional.ENSURE)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstNull(object value, string parameterName) =>
			AgainstNull(value, () => new ArgumentNullException(parameterName));

		[Conditional(Conditional.ENSURE)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstNull(object value, Func<Exception> exception) =>
			Against(value == null, exception);

		[Conditional(Conditional.ENSURE)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Against(bool condition, Func<Exception> exception)
		{
			if (condition)
			{
				throw exception();
			}
		}

		private static class Conditional
		{
			private const string TRUE = "DEBUG";
			private const string FALSE = "THIS_IS_JUST_SOME_RANDOM_STRING_THAT_IS_NEVER_DEFINED";

#if DEBUG || DEV_BUILD
			public const string ENSURE = TRUE;
#else
		public const string ENSURE = FALSE;
#endif
		}
	}
}