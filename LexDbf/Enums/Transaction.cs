namespace LexTalionis.LexDbf.Enums
{
    /// <summary>
    /// Статус транзакции
    /// </summary>
    public enum Transaction : byte
    {
        /// <summary>
        /// Завершена
        /// </summary>
        Ended = 0x00,
        /// <summary>
        /// Запущена
        /// </summary>
        Started = 0x01
    }
}