using WebAPI.Helpers;

namespace WebAPI.Filters
{
    public class SortingFilter
    {
        public string SortField { get; set; }
        public bool Ascengind { get; set; } = true;

        public SortingFilter()
        {
            SortField = "Id";
        }

        public SortingFilter(string sortField, bool ascengind)
        {
            SortField = 
                sortField ?? throw new ArgumentException(nameof(sortField));

            var sortFields = SortingHelper.GetSortFields();

            sortField = sortField.ToLower();

            if (sortFields.Select(x => x.Key).Contains(sortField))
                sortField = sortFields.Where(x => x.Key == sortField).Select(x => x.Value).FirstOrDefault();
            else
                sortField = "Id";

            SortField = sortField;
            Ascengind = ascengind;
        } 
    }
}
