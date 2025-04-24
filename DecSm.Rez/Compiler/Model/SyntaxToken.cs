namespace DecSm.Rez.Compiler.Model;

internal readonly record struct SyntaxToken(int Type, int Start, int Length)
{
    public const int Literal = 0;
    public const int Variable = 1;

    public static SyntaxToken CreateLiteral(int valueStart, int valueLength) =>
        new(Literal, valueStart, valueLength);

    public static SyntaxToken CreateVariable(int valueStart, int valueLength) =>
        new(Variable, valueStart, valueLength);
}
