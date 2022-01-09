namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public record CardLearningProgress(int Progress, FlashCard Card)
{
    private const int MaxProgress = 10;
    
    public bool Learned => Progress >= MaxProgress;
}
