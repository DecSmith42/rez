namespace DecSm.Rez.Compiler;

internal static class Executor
{
    public static string? Execute(string? input, Func<string, string?> resolution)
    {
        if (input is null)
            return null;

        if (input.Length == 0)
            return string.Empty;

        var lexemeArray = ArrayPool<Lexeme>.Shared.Rent(4096);
        var lexemeBuffer = lexemeArray.AsSpan();

        var tokenArray = ArrayPool<SyntaxToken>.Shared.Rent(4096);
        var tokenBuffer = tokenArray.AsSpan();

        var primaryTextArray = ArrayPool<char>.Shared.Rent(4096);
        var primaryTextBuffer = primaryTextArray.AsSpan();
        var secondaryTextArray = ArrayPool<char>.Shared.Rent(4096);
        var secondaryTextBuffer = secondaryTextArray.AsSpan();

        var swapBuffer = false;

        input
            .AsSpan()
            .CopyTo(primaryTextBuffer);

        var inputLength = input.Length;
        int outputLength;

        var resolutionDepth = 0;

        while (true)
        {
            if (resolutionDepth > 4096)
                throw new("Resolution depth exceeded.");

            var sourceBuffer = swapBuffer
                ? secondaryTextBuffer
                : primaryTextBuffer;

            var destBuffer = swapBuffer
                ? primaryTextBuffer
                : secondaryTextBuffer;

            var lexemeLength = Lexer.Lex(sourceBuffer[..inputLength], lexemeBuffer);
            var tokenLength = Tokenizer.TokenizeLexemes(lexemeBuffer[..lexemeLength], tokenBuffer);

            outputLength = Renderer.Render(tokenBuffer[..tokenLength], sourceBuffer[..inputLength], destBuffer, resolution);

            swapBuffer = !swapBuffer;

            var render = false;

            if (outputLength != inputLength)
                render = true;
            else
                for (var i = 0; i < outputLength; i++)
                {
                    if (destBuffer[i] == sourceBuffer[i])
                        continue;

                    render = true;

                    break;
                }

            if (!render)
                break;

            inputLength = outputLength;
            resolutionDepth++;
        }

        var output = (swapBuffer
                ? secondaryTextBuffer
                : primaryTextBuffer)[..outputLength]
            .ToString();

        ArrayPool<char>.Shared.Return(primaryTextArray);
        ArrayPool<char>.Shared.Return(secondaryTextArray);
        ArrayPool<Lexeme>.Shared.Return(lexemeArray);
        ArrayPool<SyntaxToken>.Shared.Return(tokenArray);

        return output;
    }
}
