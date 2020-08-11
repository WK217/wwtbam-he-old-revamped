using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Xml;

namespace WwtbamOld.Model
{
    public sealed class Quiz : ReadOnlyReactiveCollection<Answer>
    {
        public Quiz(XmlNode xmlNode)
        {
            Theme = xmlNode.Attributes["theme"]?.Value;

            if (byte.TryParse(xmlNode.Attributes["level"]?.Value, out byte level))
                Level = level;

            Question = new Question(xmlNode.SelectSingleNode("question"));

            var answersNode = xmlNode.SelectSingleNode("answers");

            if (Enum.TryParse(answersNode.Attributes["correct"]?.Value.ToUpper(), out AnswerID correct))
                Correct = correct;

            if (Enum.TryParse(answersNode.Attributes["fifty"]?.Value.ToUpper(), out AnswerID alternative))
                Alternative = alternative;

            A = new Answer(this, AnswerID.A, answersNode.SelectSingleNode("a").InnerText.Trim());
            B = new Answer(this, AnswerID.B, answersNode.SelectSingleNode("b").InnerText.Trim());
            C = new Answer(this, AnswerID.C, answersNode.SelectSingleNode("c").InnerText.Trim());
            D = new Answer(this, AnswerID.D, answersNode.SelectSingleNode("d").InnerText.Trim());

            _collection.AddRange(new[] { A, B, C, D });

            Comment = new Comment(xmlNode.SelectSingleNode("comment"));

            this.WhenAnyValue(quiz => quiz.Correct).Subscribe(id =>
            {
                foreach (var answer in Collection)
                    answer.IsCorrect = answer.ID == id;
            });
        }

        public Quiz()
        {
            Theme = "Тема вопроса";
            Level = 1;

            Question = new Question("Текст вопроса");

            Correct = AnswerID.A;
            Alternative = AnswerID.B;

            A = new Answer(this, AnswerID.A, "Вариант A");
            B = new Answer(this, AnswerID.B, "Вариант B");
            C = new Answer(this, AnswerID.C, "Вариант C");
            D = new Answer(this, AnswerID.D, "Вариант D");

            _collection.AddRange(new[] { A, B, C, D });

            Comment = new Comment("Комментарий");

            this.WhenAnyValue(quiz => quiz.Correct).Subscribe(id =>
            {
                foreach (var answer in Collection)
                    answer.IsCorrect = answer.ID == id;
            });
        }

        #region Properties

        [Reactive] public byte Level { get; set; }

        [Reactive] public string Theme { get; set; }

        public Question Question { get; }

        public Answer A { get; }
        public Answer B { get; }
        public Answer C { get; }
        public Answer D { get; }

        public Answer this[AnswerID id]
        {
            get
            {
                return id switch
                {
                    AnswerID.D => D,
                    AnswerID.C => C,
                    AnswerID.B => B,
                    _ => A
                };
            }
        }

        [Reactive] public AnswerID Correct { get; set; }
        [Reactive] public AnswerID Alternative { get; set; }

        public Comment Comment { get; }

        #endregion Properties

        public override string ToString() => string.Join(" | ", Question, A, B, C, D);
    }
}