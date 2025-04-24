namespace DecSm.Rez.Compiler.Model;

internal readonly record struct Lexeme(int Type, int Start, int Length)
{
    public const int Literal = 0;
    public const int OpenBrace = 1;
    public const int CloseBrace = 2;

    public static Lexeme CreateLiteral(int valueStart, int valueLength) =>
        new(Literal, valueStart, valueLength);

    public static Lexeme CreateOpenBrace(int valueStart) =>
        new(OpenBrace, valueStart, 1);

    public static Lexeme CreateCloseBrace(int valueStart) =>
        new(CloseBrace, valueStart, 1);
}
