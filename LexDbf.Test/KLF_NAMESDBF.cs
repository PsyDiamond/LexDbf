using System;
using LexTalionis.LexDbf.Common;
public class KLF_NAMESDBF
{
	[Field(Type = 'N', Length = 19, DecimalCount = 0)]
	public decimal? NAME_ID;
	[Field(Type = 'C', Length = 250, DecimalCount = 0)]
	public System.String FULL_NAME;
	[Field(Type = 'C', Length = 64, DecimalCount = 0)]
	public System.String SHORT_NAME;
}
