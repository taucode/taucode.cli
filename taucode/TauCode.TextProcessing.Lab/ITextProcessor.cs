namespace TauCode.TextProcessing.Lab
{
    public interface ITextProcessor<out TProduct>
    {
        TextProcessingResult Process(TextProcessingContext context);
        TProduct Produce(string text, int startingIndex, int length);
    }
}
