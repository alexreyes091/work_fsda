namespace app.webapi.backoffice_viajes_altairis.Common
{
    public class PagedResult<T> : Result<IEnumerable<T>> where T : class
    {
        public PagedInfo Pagination { get; }

        public PagedResult(
            IEnumerable<T> data,
            bool isSuccess,
            string error,
            string errorType,
            PagedInfo pagination) 
            : base(data, isSuccess, error, errorType)
        {
            Pagination = pagination;
        }

        public static PagedResult<T> Success(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
        {
            PagedInfo pagination = new()
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return new PagedResult<T>(data, true, string.Empty, string.Empty, pagination);
        }


    }
}
