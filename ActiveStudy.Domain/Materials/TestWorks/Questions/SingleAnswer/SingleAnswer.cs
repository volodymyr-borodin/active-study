namespace ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;

public record SingleAnswer(string QuestionId, string AnswerId) : Answer(QuestionId);
