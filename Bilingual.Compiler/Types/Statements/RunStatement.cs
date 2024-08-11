namespace Bilingual.Compiler.Types.Statements
{
    /// <summary>
    /// A run statement will run the dialogue listed and not return to the caller.
    /// </summary>
    /// <param name="Script">The name of the script to run.</param>
    public record class RunStatement(string Script) : Statement;
}
