using LexTalionis.LexDbf.Common;
// ReSharper disable CheckNamespace
// ReSharper disable CSharpWarnings::CS1591
// ReSharper disable InconsistentNaming
/// <summary>
/// Тест DBF
/// </summary>
public class KLF_NAMESDBF
// ReSharper restore InconsistentNaming
// ReSharper restore CSharpWarnings::CS1591
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// NAME_ID
    /// </summary>
	[Field(Type = 'N', Length = 19, DecimalCount = 0)]
// ReSharper disable CSharpWarnings::CS1591
// ReSharper disable InconsistentNaming
	public decimal? NAME_ID;
// ReSharper restore InconsistentNaming
// ReSharper restore CSharpWarnings::CS1591
    /// <summary>
    /// FULL_NAME
    /// </summary>
	[Field(Type = 'C', Length = 250, DecimalCount = 0)]
// ReSharper disable CSharpWarnings::CS1591
// ReSharper disable InconsistentNaming
	public System.String FULL_NAME;
// ReSharper restore InconsistentNaming
// ReSharper restore CSharpWarnings::CS1591
    /// <summary>
    /// SHORT_NAME
    /// </summary>
	[Field(Type = 'C', Length = 64, DecimalCount = 0)]
// ReSharper disable CSharpWarnings::CS1591
// ReSharper disable InconsistentNaming
	public System.String SHORT_NAME;
// ReSharper restore InconsistentNaming
// ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'D')]
// ReSharper disable CSharpWarnings::CS1591
// ReSharper disable InconsistentNaming
    public System.DateTime? DATEF;
// ReSharper restore InconsistentNaming
// ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'D')]
    // ReSharper disable CSharpWarnings::CS1591
    // ReSharper disable InconsistentNaming
    public System.DateTime? DATEEmpty;
    // ReSharper restore InconsistentNaming
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'T')]
    // ReSharper disable CSharpWarnings::CS1591
    // ReSharper disable InconsistentNaming
    public System.DateTime? TimeFull;
    // ReSharper restore InconsistentNaming
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'T')]
    // ReSharper disable CSharpWarnings::CS1591
    // ReSharper disable InconsistentNaming
    public System.DateTime? TimeEmpty;
    // ReSharper restore InconsistentNaming
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'N')]
    // ReSharper disable CSharpWarnings::CS1591
    // ReSharper disable InconsistentNaming
    public decimal? Dec;
    // ReSharper restore InconsistentNaming
    // ReSharper restore CSharpWarnings::CS1591
    // ReSharper restore CSharpWarnings::CS1591
    [Field(Type = 'L')]
    // ReSharper disable CSharpWarnings::CS1591
    // ReSharper disable InconsistentNaming
    public bool Bool;
    // ReSharper restore InconsistentNaming
    // ReSharper restore CSharpWarnings::CS1591
}
