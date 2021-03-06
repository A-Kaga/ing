﻿using System;

namespace Sakuno.ING.Composition
{
    public interface IResolver
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
    }
}
