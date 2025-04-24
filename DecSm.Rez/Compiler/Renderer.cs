namespace DecSm.Rez.Compiler;

internal static class Renderer
{
    public static int Render(
        Span<SyntaxToken> tokens,
        ReadOnlySpan<char> inputText,
        Span<char> outputText,
        Func<string, string?> resolution)
    {
        foreach (var token in tokens)
        {
            if (token.Type != SyntaxToken.Variable)
                continue;

            var variableStart = token.Start - 1;
            var variableLength = token.Length + 2;

            var variable = inputText
                .Slice(variableStart, variableLength)
                .ToString();

            var resolutionInput = inputText
                .Slice(token.Start, token.Length)
                .ToString();

            var resolutionResult = resolution(resolutionInput) ?? variable;

            if (resolutionResult == variable)
                continue;

            inputText[..variableStart]
                .CopyTo(outputText);

            resolutionResult
                .AsSpan()
                .CopyTo(outputText[variableStart..]);

            inputText[(variableStart + variableLength)..]
                .CopyTo(outputText[(variableStart + resolutionResult.Length)..]);

            return variableStart + resolutionResult.Length + inputText.Length - (variableStart + variableLength);
        }

        inputText.CopyTo(outputText);

        return inputText.Length;
    }
}
