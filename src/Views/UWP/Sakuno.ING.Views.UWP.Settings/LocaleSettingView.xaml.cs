﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sakuno.ING.Composition;
using Sakuno.ING.Settings;
using Windows.UI.Xaml.Controls;

namespace Sakuno.ING.Views.UWP.Settings
{
    public sealed partial class LocaleSettingView : UserControl
    {
        private LocaleSetting Instance = StaticResolver.Instance.Resolve<LocaleSetting>();
        private KeyValuePair<string, string>[] Languages = Windows.Globalization.ApplicationLanguages.ManifestLanguages
            .Select(x => new KeyValuePair<string, string>(x, new CultureInfo(x).NativeName))
            .Prepend(new KeyValuePair<string, string>(string.Empty, "(default)"))
            .ToArray();

        public LocaleSettingView()
        {
            this.InitializeComponent();
        }
    }
}
