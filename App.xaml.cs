using Microsoft.Win32;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using WwtbamOld.Interactions;
using WwtbamOld.Media.Audio;
using WwtbamOld.Model;
using WwtbamOld.View;
using WwtbamOld.ViewModel;

namespace WwtbamOld;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Methods

    public static string GetWindowTitle(string name) => $"«Кто хочет стать миллионером?» Home Edition Old Revamped :: {name}";

    private static void InitializeInteractions()
    {
        // Открытие файла с пакетом вопросов
        DialogWindowInteractions.ShowOpenQuizbaseDialog.RegisterHandler(context =>
        {
            OpenFileDialog openFileDialog = new()
            {
                DefaultExt = ".xml",
                Filter = "XML/JSON-файлы (*.xml; *.json)|*.xml;*.json|Все файлы (*.*)|*.*",
                Title = "Выбор файла пакета вопросов",
                AddExtension = true,
                CheckFileExists = true,
#if NET6_0_OR_GREATER
                InitialDirectory = Path.GetFullPath(Environment.ProcessPath)
#else
                InitialDirectory = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName)
#endif
            };

            if (openFileDialog.ShowDialog() == true)
                context.SetOutput(ResourceManager.LoadQuizzesFromFile(openFileDialog.FileName));
            else
                context.SetOutput(null);
        });
    }

    private void AppStartup(object sender, StartupEventArgs e)
    {
        InitializeInteractions();
        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
        Locator.CurrentMutable.RegisterConstant(new ObjectToDescriptionTypeConverter(), typeof(IBindingTypeConverter));

        Game game = new();
        MainViewModel mainViewModel = new(game);

        HostView hostView = new() { ViewModel = mainViewModel.Host };
        hostView.Closed += WindowClosed;
        hostView.Show();

        ScreenView screenView = new() { ViewModel = mainViewModel.Screen };
        screenView.Closed += WindowClosed;
        screenView.Show();
    }

    private void WindowClosed(object sender, EventArgs e)
    {
        AudioManager.Instance.Dispose();
        Shutdown();
    }

    #endregion Methods
}