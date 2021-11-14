namespace ActiveStudy.Domain.Materials.TestWorks;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text">Text visible for students</param>
/// <param name="Description">Description visible for teachers</param>
/// <param name="MaxScore">How much student can scored get for right answer</param>
public abstract record Question(string Id, string Text, string Description, decimal MaxScore)
{
    public abstract decimal CalculateGainedScore(Answer answer);
}
