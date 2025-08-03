namespace BrightBudget.API.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T> { Success = true, Data = data };
        }

        public static ApiResponse<T> Fail(string error)
        {
            return new ApiResponse<T> { Success = false, Error = error };
        }

        public static ApiResponse<T> Fail(IEnumerable<string> errors)
        {
            return new ApiResponse<T> { Success = false, Error = string.Join("; ", errors) };
        }
    }
}