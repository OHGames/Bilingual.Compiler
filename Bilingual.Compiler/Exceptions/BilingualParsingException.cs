using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilingual.Compiler.Exceptions
{
    /// <summary>
    /// When parsing goes awry
    /// </summary>
    internal class BilingualParsingException : Exception
    {
        public BilingualParsingException() { }
        public BilingualParsingException(string message) : base(message) { }
        public BilingualParsingException(string message, Exception inner) : base(message, inner) { }
    }
}
