namespace BrightBudget.API.Common
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T[]? Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedResponse() { }

        public PagedResponse(T[]? data, int pageNumber, int pageSize, int totalRecords, string? message = null)
        {
            Success = true;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);
            Message = message;
        }
    }
}
