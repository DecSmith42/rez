namespace DecSm.Rez.Compiler;

internal static class Tokenizer
{
    public static int TokenizeLexemes(Span<Lexeme> lexemes, Span<SyntaxToken> tokens)
    {
        var lexemeIndex = 0;
        var tokenIndex = 0;

        while (lexemeIndex < lexemes.Length)
        {
            var result = lexemes[lexemeIndex].Type == Lexeme.OpenBrace
                ? TokenizeNonLiteralLexeme(lexemes[lexemeIndex..], tokens[tokenIndex..])
                : TokenizeLiteralLexeme(lexemes[lexemeIndex..], tokens[tokenIndex..]);

            lexemeIndex += result.ConsumedLexemes;
            tokenIndex += result.ProducedTokens;
        }

        return tokenIndex;
    }

    private static SyntaxResult TokenizeLiteralLexeme(Span<Lexeme> lexemes, Span<SyntaxToken> tokens)
    {
        var lexeme = lexemes[0];
        tokens[0] = SyntaxToken.CreateLiteral(lexeme.Start, lexeme.Length);

        return new(1, 1);
    }

    private static SyntaxResult TokenizeNonLiteralLexeme(Span<Lexeme> lexemes, Span<SyntaxToken> tokens)
    {
        if (lexemes.Length == 1)
        {
            tokens[0] = SyntaxToken.CreateLiteral(lexemes[0].Start, lexemes[0].Length);

            return new(1, 1);
        }

        var firstLexeme = lexemes[1];

        var lexemeIndex = 1;
        var tokenIndex = 0;

        while (lexemeIndex < lexemes.Length)
        {
            var lexeme = lexemes[lexemeIndex];

            if (lexeme.Type == Lexeme.CloseBrace)
            {
                tokens[tokenIndex++] = SyntaxToken.CreateVariable(firstLexeme.Start, lexeme.Start - firstLexeme.Start);

                return new(lexemeIndex + 1, tokenIndex);
            }

            var result = lexeme.Type == Lexeme.OpenBrace
                ? TokenizeNonLiteralLexeme(lexemes[lexemeIndex..], tokens[tokenIndex..])
                : TokenizeLiteralLexeme(lexemes[lexemeIndex..], tokens[tokenIndex..]);

            lexemeIndex += result.ConsumedLexemes;
            tokenIndex += result.ProducedTokens;
        }

        // Handle case where there is no closing brace

        for (var i = 0; i < tokenIndex; i++)

            // If there are any nested non-literals, then they will be evaluated first so we can just return
            if (tokens[i].Type == SyntaxToken.Variable)
                return new(lexemes.Length, tokenIndex);

        var charLength = 0;

        foreach (var lexeme in lexemes)
            charLength += lexeme.Length;

        // If there are no nested non-literals, then we can just treat the whole thing as a literal
        tokens[0] = SyntaxToken.CreateLiteral(lexemes[0].Start, charLength);

        return new(lexemes.Length, 1);
    }
}
