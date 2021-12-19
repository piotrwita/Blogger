namespace WebAPI.Helpers
{
    public class SortingHelper
    {
        public static KeyValuePair<string, string>[] GetSortFields()
        {
            return new[] { SortFields.Title, SortFields.CreationDate };
        }
    }

    /// <summary>
    /// Pomocnicza klasa określająca po których polach można dokonać sortowania 
    /// przy okazji właściwości będą mapowały nazwę pola przesyłaną od klienta na nazwę pola wykorzystywaną w sortowaniu
    /// </summary>
    public class SortFields
    {
        public static KeyValuePair<string, string> Title { get; } = new KeyValuePair<string, string>("title", "Title");
        public static KeyValuePair<string, string> CreationDate { get; } = new KeyValuePair<string, string>("creationdate", "Created");
    }
}
