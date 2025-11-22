namespace RiseApi.DTOs
{
    public class PagedResponse<T>
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public List<T> data { get; set; } = new();
        public object links { get; set; } = null!;
    }
}
