using Microsoft.Win32;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Windows;
using WwtbamOld.Interactions;
using WwtbamOld.Media.Audio;
using WwtbamOld.Model;
using WwtbamOld.View;
using WwtbamOld.ViewModel;

namespace WwtbamOld
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string AppName => @"«Кто хочет стать миллионером?» Home Edition Old Revamped";

        private void AppStartup(object sender, StartupEventArgs e)
        {
            InitializeInteractions();
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
            Locator.CurrentMutable.RegisterConstant(new UriToImageSourceTypeConverter(), typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(new ObjectToDescriptionTypeConverter(), typeof(IBindingTypeConverter));

            Game game = new Game();
            MainViewModel mainViewModel = new MainViewModel(game);

            HostView hostView = new HostView { ViewModel = mainViewModel.Host };
            hostView.Closed += WindowClosed;
            hostView.Show();

            ScreenView screenView = new ScreenView { ViewModel = mainViewModel.Screen };
            screenView.Closed += WindowClosed;
            screenView.Show();

            hostView.Activate();
            hostView.Topmost = true;
            hostView.Topmost = false;
        }

        private void InitializeInteractions()
        {
            // Показ тестового сообщения
            MessageInteractions.ShowMessage.RegisterHandler(context =>
            {
                MessageBox.Show(context.Input);
                context.SetOutput(Unit.Default);
            });

            // Открытие файла с пакетом вопросов
            DialogWindowInteractions.ShowOpenQuizbaseDialog.RegisterHandler(context =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    DefaultExt = ".xml",
                    Filter = "XML-файлы (.xml)|*.xml",
                    Title = "Выбор файла пакета вопросов",
                    AddExtension = true,
                    CheckFileExists = true,
                    InitialDirectory = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName)
                };

                if (openFileDialog.ShowDialog() == true)
                    context.SetOutput(ResourceManager.LoadQuizzesFromFile(openFileDialog.FileName));
            });
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            AudioManager.Instance.CleanupPlayback();
            Shutdown();
        }
    }
}