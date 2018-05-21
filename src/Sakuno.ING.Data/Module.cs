﻿using System.Runtime.CompilerServices;
using Sakuno.ING.Composition;
using Sakuno.ING.Settings;

[assembly: InternalsVisibleTo("Sakuno.ING.Data.Design")]

namespace Sakuno.ING.Data
{
    internal class Module : IExposableModule
    {
        public void Expose(IBuilder builder)
        {
            builder.RegisterService<SettingsManager, ISettingsManager>();
        }

        public void Initialize(IResolver resolver) { }
    }
}
