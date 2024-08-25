using Newtonsoft.Json;
using System.Collections;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record class BlockedStatement(Block Block) : Statement, IEnumerable<Statement>
    {
        public Statement this[int i]
        {
            get => Block.Statements[i];
            set => Block.Statements[i] = value;
        }

        [JsonIgnore]
        public int StatementCount => Block.Statements.Count;

        public IEnumerator<Statement> GetEnumerator()
        {
            return Block.Statements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Block.Statements.GetEnumerator();
        }
    }
}
