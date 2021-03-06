﻿using Sakuno.ING.Composition;
using Sakuno.ING.Settings;
using Sakuno.ING.Shell;

namespace Sakuno.ING.Views.UWP.Settings
{
    internal class Module : IModule
    {
        public void Initialize(IResolver resolver)
        {
            var shell = resolver.Resolve<IShell>();
            shell.RegisterSettingView(typeof(LocaleSettingView), SettingCategory.Appearance);
        }
    }
}
