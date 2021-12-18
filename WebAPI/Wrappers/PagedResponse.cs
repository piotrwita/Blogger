namespace WebAPI.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        /// <summary>
        /// Właściwość określa numer strony
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Właściwość określa rozmiar strony
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Właściwość określa liczbę wszystkich stron
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Właściwość określa liczbę wszystkich rekordów
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Właściwość określa czy następna strona istnieje
        /// </summary>
        public bool NextPage { get; set; }

        /// <summary>
        /// Właściwość określa czy poprzednia strona istnieje
        /// </summary>
        public bool PreviousPage { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)//, int totalPages, int totalRecords, bool nextPage, bool previousPage)
        {
            Data =
                data ?? throw new ArgumentNullException(nameof(data));

            PageNumber = pageNumber;
            PageSize = pageSize;

            Succeeded = true;

            //TotalPages = totalPages;
            //TotalRecords = totalRecords;
            //NextPage = nextPage;
            //PreviousPage = previousPage;
        }
    }
}
