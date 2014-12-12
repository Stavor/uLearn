using System;
using System.IO;
using NUnit.Framework;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts
{
    public class PeerAssasmentRepositoryTest
    {
        [SetUp]
        public void SetUp()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var appDataDirectory = Path.Combine(baseDirectory.Replace("\\bin", ""), "App_Data");

            AppDomain.CurrentDomain.SetData("DataDirectory", appDataDirectory);

            repository = new PeerAsssasmentAnswerRepository();
            CleanAllDatas();
            CheckCorrectAnswerModel(new AnswerModel(), repository.GetOrCreate(answerId));
        }

        [Test]
        public void PropositionUpdateTest()
        {
            var proposition = new PropositionModel
            {
                Text = "Утверждение!"
            };
            repository.UpdateAnswerBy(answerId, proposition);
            var expectedAnswer = new AnswerModel
            {
                Proposition = new PropositionModel
                {
                    Text = proposition.Text
                }
            };
            var answer = repository.GetOrCreate(answerId);
            CheckCorrectAnswerModel(expectedAnswer, answer);
        }

        [Test]
        public void ReviewUpdateTest()
        {
            var review = new ReviewModel
            {
                Text = "А тут текст Review"
            };

            repository.UpdateAnswerBy(answerId, review);
            var expectedAnswer = new AnswerModel
            {
                Review = new ReviewModel
                {
                    Text = review.Text
                }
            };
            var answer = repository.GetOrCreate(answerId);
            CheckCorrectAnswerModel(expectedAnswer, answer);
        }

        [Test]
        public void ReviewWithMarkUpdateTest()
        {
            var reviewWitMark = new ReviewModel
            {
                Text = "И че? Как то не содержательно!",
                Marks = new[]
                {
                    new MarkModel
                    {
                        Criterion = "Оценка",
                        Mark = "Неуд"
                    }
                }
            };

            repository.UpdateAnswerBy(answerId, reviewWitMark);
            var expectedAnswer = new AnswerModel
            {
                Review = new ReviewModel
                {
                    Text = reviewWitMark.Text,
                    Marks = reviewWitMark.Marks
                }
            };
            var answer = repository.GetOrCreate(answerId);
            CheckCorrectAnswerModel(expectedAnswer, answer);
        }

        private static void CheckCorrectAnswerModel(AnswerModel expectedAnswer, AnswerModel actualAnswer)
        {
            expectedAnswer = Extend(expectedAnswer);
            actualAnswer = Extend(actualAnswer);
            Assert.AreEqual(expectedAnswer.Proposition.Text, actualAnswer.Proposition.Text, "Неожиданный текст утверждения.");
            Assert.AreEqual(expectedAnswer.Review.Text, actualAnswer.Review.Text, "Неожиданный текст ревью.");
            Assert.AreEqual(expectedAnswer.Review.Marks.Length, actualAnswer.Review.Marks.Length, "Не ожиданное кол-во оценок.");
            for (var i = 0; i < expectedAnswer.Review.Marks.Length; i++)
            {
                var expMark = expectedAnswer.Review.Marks[i];
                var actMark = actualAnswer.Review.Marks[i];
                Assert.AreEqual(expMark.Mark, actMark.Mark, "Нежиданная оценка.");
                Assert.AreEqual(expMark.Criterion, actMark.Criterion, "Неожиданный критерий оценки.");
            }
        }

        private static AnswerModel Extend(AnswerModel answer)
        {
            answer = answer ?? new AnswerModel();
            answer.Proposition = answer.Proposition ?? new PropositionModel();
            answer.Review = answer.Review ?? new ReviewModel();
            answer.Review.Marks = answer.Review.Marks ?? new MarkModel[0];
            return answer;
        }

        private static void CleanAllDatas()
        {
            CleanSet<Answer>();
            CleanSet<Review>();
            CleanSet<Proposition>();
            CleanSet<Mark>();
        }

        private static void CleanSet<T>()
            where T : class
        {
            var db = new ULearnDb();
            var set = db.Set<T>();
            set.RemoveRange(set);
            db.SaveChanges();
        }

        private IPeerAsssasmentAnswerRepository repository;

        private readonly AnswerId answerId = new AnswerId
        {
            UserId = "userId",
            CourseId = "courseId",
            SlideId = "slideId"
        };
    }
}