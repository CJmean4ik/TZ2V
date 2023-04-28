namespace TZ2V.Parser
{
    /// <summary>
    /// Парсер
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IParser<T>
    {
        Task<T> ParseAsync(int index);
    }
}
