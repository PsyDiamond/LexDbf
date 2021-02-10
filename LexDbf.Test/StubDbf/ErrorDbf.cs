using LexTalionis.LexDbf.Common;

namespace LexDbf.Test.StubDbf
{
    class ErrorDbf
    {
        /// <summary>
        /// NAME_ID
        /// </summary>
// ReSharper disable InconsistentNaming
        public string NAME_ID;
// ReSharper restore InconsistentNaming
        // ReSharper restore CSharpWarnings::CS1591
        [Field(Type = 'N')]
        // ReSharper disable CSharpWarnings::CS1591
        // ReSharper disable InconsistentNaming
        public short Short = 9;
        // ReSharper restore InconsistentNaming
        // ReSharper restore CSharpWarnings::CS1591
    }
}
