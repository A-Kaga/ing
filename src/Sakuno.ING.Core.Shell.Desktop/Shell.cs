﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Sakuno.ING.Services;
using Sakuno.ING.Settings;
using Sakuno.ING.ViewModels;
using Sakuno.ING.ViewModels.Layout;

namespace Sakuno.ING.Shell
{
    class Shell : IShell
    {
        MainWindowVM _mainWindowVM;
        private readonly ILocalizationService localization;
        private readonly ITextStreamProvider provider;
        private readonly string localeName;
        private readonly FontFamily userFont;

        public Shell(ILocalizationService localization, LocaleSetting locale, ITextStreamProvider provider)
        {
            _mainWindowVM = new MainWindowVM()
            {
                Title = "Intelligent Naval Gun",
            };
            this.localization = localization;
            this.provider = provider;
            localeName = locale.Language.Value;
            var userFontName = locale.UserLanguageFont.Value;
            if (!string.IsNullOrEmpty(userFontName))
                userFont = new FontFamily(userFontName);
        }

        private readonly Dictionary<string, (Type ViewType, bool IsFixedSize, bool SingleWindowRecommended)> views = new Dictionary<string, (Type, bool, bool)>();
        private readonly List<(Type ViewType, SettingCategory Category)> settingViews = new List<(Type, SettingCategory)>();
        private bool started;

        public void RegisterView(Type viewType, string id, bool isFixedSize = true, bool singleWindowRecommended = false)
        {
            if (started) throw new InvalidOperationException("Shell already started.");
            views.Add(id, (viewType, isFixedSize, singleWindowRecommended));
        }

        public void RegisterSettingView(Type viewType, SettingCategory category = SettingCategory.Misc)
        {
            if (started) throw new InvalidOperationException("Shell already started.");
            settingViews.Add((viewType, category));
        }

        private LayoutRoot layout;
        private MainWindow main;
        private SettingsWindow settings;

        public void Run()
        {
            started = true;

            layout = new LayoutRoot();
            layout.Entries.Add(new LayoutItem { Id = "Fleets" });

            main = new MainWindow() { DataContext = _mainWindowVM };
            InitWindow(main);
            main.Settings.Click += (_, __) =>
            {
                if (settings == null)
                {
                    settings = new SettingsWindow
                    {
                        DataContext = settingViews
                            .GroupBy(e => e.Category)
                            .OrderBy(e => e.Key)
                            .Select(e => new
                            {
                                Title = localization.GetLocalized("SettingCategory", e.Key.ToString()) ?? e.Key.ToString(),
                                Content = e.Select(t => Activator.CreateInstance(t.ViewType)).ToArray()
                            }).ToArray()
                    };
                    InitWindow(settings);
                    settings.Closed += (___, ____) => settings = null;
                }
                settings.Show();
                settings.Activate();
            };
            Arrange();

            var app = new DesktopApp
            {
                MainWindow = main,
                ShutdownMode = ShutdownMode.OnMainWindowClose
            };

            app.MainWindow.Show();

            provider.Enabled = true;

            app.Run();
        }

        private void InitWindow(Window window)
        {
            if (!string.IsNullOrEmpty(localeName))
                window.Language = XmlLanguage.GetLanguage(localeName);
            if (userFont != null)
                window.FontFamily = userFont;
        }

        private string GetTitle(LayoutBase item)
            => item.Title
            ?? localization.GetLocalized("ViewTitle", item.Id)
            ?? item.Id;

        private void Arrange()
        {
            foreach (var entry in layout.Entries)
                if (main.MainContent.Content == null)
                    main.MainContent.Content = BuildLayout(entry);
                else
                {
                    var button = new Button { Content = GetTitle(entry), Tag = entry };
                    button.Click += (sender, e) =>
                    {
                        var le = (sender as Button).Tag as LayoutBase;
                        var window = new Window { Content = BuildLayout(le) };
                        InitWindow(window);
                        window.Show();
                        window.Activate();
                    };
                    main.Switcher.Children.Add(button);
                }
        }

        private FrameworkElement BuildLayout(LayoutBase layout)
        {
            switch (layout)
            {
                case TabLayout tab:
                    var tabcontrol = new TabControl { Name = tab.Id };
                    foreach (var child in tab.Children)
                        tabcontrol.Items.Add(new TabItem
                        {
                            Header = GetTitle(child),
                            Content = BuildLayout(child)
                        });
                    return tabcontrol;

                case LayoutItem item:
                    if (!views.TryGetValue(item.Id, out var descriptor))
                        return null;
                    return (FrameworkElement)Activator.CreateInstance(descriptor.ViewType);
                default:
                    return null;
            }
        }
    }
}
