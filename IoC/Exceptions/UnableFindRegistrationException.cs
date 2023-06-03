using System;
using System.Reflection;

namespace Depra.IoC.Exceptions
{
    internal sealed class UnableFindRegistrationException : Exception
    {
        private const string MESSAGE_FORMAT = "Unable to find registration for {0}";

        public UnableFindRegistrationException(MemberInfo service) :
            base(string.Format(MESSAGE_FORMAT, service.Name)) { }
    }
}