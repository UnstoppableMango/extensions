using System;
using JetBrains.Annotations;

namespace KG.DCX.Extensions.Cqrs
{
    internal delegate object ServiceFactory([NotNull] Type type);
}
