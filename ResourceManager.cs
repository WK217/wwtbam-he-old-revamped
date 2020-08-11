using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using WwtbamOld.Model;

namespace WwtbamOld
{
    internal static class ResourceManager
    {
        #region Вопросы

        public static IEnumerable<Quiz> LoadQuizzesFromFile(string fileName)
        {
            using FileStream stream = File.Open(fileName, FileMode.Open);
            using StreamReader streamReader = new StreamReader(stream);
            return LoadQuizzes(streamReader.ReadToEnd());
        }

        public static IEnumerable<Quiz> LoadQuizzesDefault()
        {
            //return LoadQuizzes("<?xml version=\"1.0\" encoding=\"utf-8\" ?><quizzes><quiz theme=\"Рубашки\" level=\"1\"><question><text>Какие рубашки не имеют выраженной системы размеров?</text></question><answers correct=\"d\"><a>Детские</a><b>Форменные</b><c>Ночные</c><d>Смирительные</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Из песни слова не выкинешь\" level=\"2\"><question><text>Какое действие предагается совершить в припеве песни «Дубинушка»?</text></question><answers correct=\"c\"><a>Ахнуть</a><b>Охнуть</b><c>Ухнуть</c><d>Ёкнуть</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Мифология\" level=\"3\"><question><text>Куда критский царь Минос заточил Минотавра, обязав афинян кормить чудовище юношами и девушками?</text></question><answers correct=\"a\"><a>Лабиринт</a><b>Галерея</b><c>Катакомбы</c><d>Подвал</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Вина\" level=\"4\"><question><text>Какое из этих вин получило в народе прозвище «бормотуха»?</text></question><answers correct=\"d\"><a>Сухое</a><b>Вермут</b><c>Мадера</c><d>Плодово-ягодное</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Английский язык\" level=\"5\"><question><text>Какой день недели англичане называют Tuesday?</text></question><answers correct=\"b\"><a>Понедельник</a><b>Вторник</b><c>Среда</c><d>Четверг</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Страхование\" level=\"6\"><question><text>Как в договоре страхования называется сторона, страхующая свой имущественный интерес?</text></question><answers correct=\"a\"><a>Страхователь</a><b>Страховальщик</b><c>Страховщик</c><d>Страхованный</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Спорт\" level=\"7\"><question><text>Какое из этих массовых спортивных состязаний были учреждены организациями рабочих?</text></question><answers correct=\"b\"><a>Олимпиада</a><b>Спартакиада</b><c>Универсиада</c><d>Игры Доброй воли</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Литература\" level=\"8\"><question><text>Как называется один из сборников повестей Н. В. Гоголя?</text></question><answers correct=\"c\"><a>«Баккара»</a><b>«Оазис»</b><c>«Арабески»</c><d>«Нирвана»</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Гагарин\" level=\"9\"><question><text>Какое внеочередное воинское звание получил за свой полёт в космос Юрий Гагарин?</text></question><answers correct=\"a\"><a>Майор</a><b>Капитан</b><c>Подполковник</c><d>Генерал-лейтенант</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Римские цифры\" level=\"10\"><question><text>Какой год указан в римском числительном MCMXCIX?</text></question><answers correct=\"c\"><a>1899</a><b>1909</b><c>1999</c><d>2009</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Этнография\" level=\"11\"><question><text>Какой народ ранее назывался тунгусами?</text></question><answers correct=\"c\"><a>Чукчи</a><b>Калмыки</b><c>Эвенки</c><d>Якуты</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Греческий пантеон\" level=\"12\"><question><text>Кто из перечисленных мифологических богов не был вооружён луком и стрелами?</text></question><answers correct=\"d\"><a>Купидон</a><b>Аполлон</b><c>Диана</c><d>Дионис</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Опера\" level=\"13\"><question><text>Какую оперу поставил на сцене лондонского Ковент-Гардена Андрей Тарковский?</text></question><answers correct=\"d\"><a>«Иван Сусанин»</a><b>«Евгений Онегин»</b><c>«Пиковая дама»</c><d>«Борис Годунов»</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Геральдика\" level=\"14\"><question><text>Какой из этих цветов каноническая геральдика считает дополнительным, используя его лишь в редких случаях?</text></question><answers correct=\"c\"><a>Зелёный</a><b>Чёрный</b><c>Пурпурный</c><d>Красный</d></answers><comment><text>Текст комментария.</text></comment></quiz><quiz theme=\"Российская империя\" level=\"15\"><question><text>Кто был первым военным министром Российской империи?</text></question><answers correct=\"d\"><a>Аракчеев</a><b>Барклай-де-Толли</b><c>Коновницын</c><d>Вязмитинов</d></answers><comment><text>Текст комментария.</text></comment></quiz></quizzes>");
            return LoadQuizzes(GetResourceFileContents("quizzes", "xml"));
        }

        public static IEnumerable<Quiz> LoadQuizzes(string xml)
        {
            var quizzes = new List<Quiz>();

            var xmlDocument = new XmlDocument { PreserveWhitespace = false };
            xmlDocument.LoadXml(xml);
            foreach (XmlNode quizNode in xmlDocument.SelectNodes("//quizzes/quiz"))
                quizzes.Add(new Quiz(quizNode));

            return quizzes;
        }

        #endregion Вопросы

        #region Графика

        private const string GRAPHICS_PREFIX = @"Resources\Graphics";

        public static string GetFileImagePath(string folders, string fileName, string extension)
            => Path.GetFullPath(Path.Combine(GRAPHICS_PREFIX, folders, $"wwtbam {fileName}.{extension}"));

        public static ImageSource GetFileImageSource(string folders, string fileName, string extension)
            => GetFileImageSource(GetFileImagePath(folders, fileName, extension));

        public static ImageSource GetFileImageSource(string filePath)
            => GetFileImageSource(new Uri(filePath, UriKind.Absolute));

        private static ImageSource GetFileImageSource(Uri uri)
            => (ImageSource)new ImageSourceConverter().ConvertFrom(uri);

        #endregion Графика

        #region Ресурсы

        public static string GetResourceName(string fileName, ResourceType resourceType, string extension, params string[] folders)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var sep = '.';

            stringBuilder.Append("WwtbamOld");
            stringBuilder.Append(sep);

            if (resourceType == ResourceType.None)
            {
                if (folders.Length > 0)
                {
                    stringBuilder.Append(string.Join('.', folders).Replace(' ', '_'));
                    stringBuilder.Append(sep);
                }

                stringBuilder.AppendJoin(sep, $"{fileName}.{extension}");
            }
            else
            {
                stringBuilder.AppendJoin(sep, "Resources", resourceType.ToString());

                if (folders.Length > 0)
                {
                    stringBuilder.Append(sep);
                    stringBuilder.Append(string.Join('.', folders).Replace(' ', '_'));
                }

                stringBuilder.Append(sep);
                stringBuilder.AppendJoin(sep, $"{(resourceType != ResourceType.Fonts ? "wwtbam " : "")}{fileName}.{extension}");
            }

            return stringBuilder.ToString();
        }

        private static Stream GetStreamByResourceName(string resourceName)
            => Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        public static string GetFontName(string fileName, string extension, params string[] folders)
            => GetResourceName(fileName, ResourceType.Fonts, extension, folders);

        /*public static FontFamily GetFont(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName);
        }*/

        public static string GetResourceFileContents(string fileName, string extension, params string[] folders)
            => GetResourceFileContents(GetResourceName(fileName, ResourceType.None, extension, folders));

        public static string GetResourceFileContents(string resourceName)
            => GetResourceFileContents(GetStreamByResourceName(resourceName));

        public static string GetResourceFileContents(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static string GetResourceGraphicsName(string fileName, string extension, params string[] folders)
            => GetResourceName(fileName, ResourceType.Graphics, extension, folders);

        public static ImageSource GetImageSource(string fileName, string extension, params string[] folders)
            => GetImageSource(GetResourceGraphicsName(fileName, extension, folders));

        public static ImageSource GetImageSource(string resourceName)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = GetStreamByResourceName(resourceName);
            image.EndInit();

            return image;
        }

        public static string GetResourceAudioName(string fileName, string extension = "mp3", params string[] folders)
            => GetResourceName(fileName, ResourceType.Audio, extension, folders);

        public static Stream GetAudioStream(string fileName, string extension = "mp3", params string[] folders)
            => GetAudioStream(GetResourceAudioName(fileName, extension, folders));

        public static Stream GetAudioStream(string resourceName)
            => GetStreamByResourceName(resourceName);

        #endregion Ресурсы
    }

    internal enum ResourceType : byte
    {
        None,
        Audio,
        Fonts,
        Graphics,
    }
}