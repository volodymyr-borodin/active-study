using System.Collections.Generic;

namespace ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;

public record MultiAnswer(string QuestionId, IEnumerable<string> AnswerIds) : Answer(QuestionId);
