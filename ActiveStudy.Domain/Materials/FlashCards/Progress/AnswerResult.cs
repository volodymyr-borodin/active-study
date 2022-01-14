using System;
using System.Linq;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record AnswerResult(NewAnswer Answer, FlashCard Card)
{
    public bool IsCorrect => CompareSentences(StringToClearSentence(Card.Term), StringToClearSentence(Answer.Answer));

    private static bool CompareSentences(string[] a, string[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (var i = 0; i < a.Length; i++)
        {
            if (!a[i].Equals(b[i], StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private static string[] StringToClearSentence(string t)
    {
        return t.Split('/')
            .Select(a => string.Join(' ', a.Trim().Split(' ')
                .Where(w => w != "a" && w != "an" && w != "the" && w != "to")))
            .OrderBy(a => a)
            .ToArray();
    }
}
