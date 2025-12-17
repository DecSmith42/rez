namespace DecSm.Rez.Compiler;

internal static class Lexer
{
    public static int Lex(ReadOnlySpan<char> input, Span<Lexeme> output)
    {
        var lexemeCount = 0;
        var checkpointIndex = 0;
        var workingIndex = 0;
        var escaped = false;

        while (workingIndex < input.Length)
        {
            switch (input[workingIndex])
            {
                case '\\' when !escaped:
                {
                    escaped = true;
                    workingIndex++;

                    continue;
                }

                case '{' when !escaped:
                {
                    if (workingIndex > checkpointIndex)
                        output[lexemeCount++] = Lexeme.CreateLiteral(checkpointIndex, workingIndex - checkpointIndex);

                    output[lexemeCount++] = Lexeme.CreateOpenBrace(workingIndex);
                    workingIndex++;
                    checkpointIndex = workingIndex;

                    continue;
                }

                case '}' when !escaped:
                {
                    if (workingIndex > checkpointIndex)
                        output[lexemeCount++] = Lexeme.CreateLiteral(checkpointIndex, workingIndex - checkpointIndex);

                    output[lexemeCount++] = Lexeme.CreateCloseBrace(workingIndex);
                    workingIndex++;
                    checkpointIndex = workingIndex;

                    continue;
                }

                default:
                {
                    escaped = false;
                    workingIndex++;

                    continue;
                }
            }
        }

        if (workingIndex > checkpointIndex)
            output[lexemeCount++] = Lexeme.CreateLiteral(checkpointIndex, workingIndex - checkpointIndex);

        return lexemeCount;
    }
}
