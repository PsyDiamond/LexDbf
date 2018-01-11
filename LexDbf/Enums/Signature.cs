namespace LexTalionis.LexDbf.Enums
{
    /// <summary>
    /// Тип базы
    /// </summary>
    public enum Signature : byte
    {
        /// <summary>
        /// FoxPro
        /// </summary>
        FoxBase = 0x02,
        /// <summary>
        /// Только DBF
        /// </summary>
        FileWithoutDBT = 0x03,
        /// <summary>
        /// тоже FoxPro
        /// </summary>
        VisualFoxPro = 0x30
    }
}