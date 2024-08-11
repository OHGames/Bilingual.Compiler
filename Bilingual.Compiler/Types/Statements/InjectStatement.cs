namespace Bilingual.Compiler.Types.Statements
{
    /// <summary>
    /// An inject statement will add the script into the current scope.
    /// All variables in the calling script will be accessable from the injected script.
    /// The calling script will continue to run after the injected script is finished.
    /// </summary>
    /// <param name="Script">The name of the script to inject.</param>
    public record class InjectStatement(string Script) : Statement;
}
