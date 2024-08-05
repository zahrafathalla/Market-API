using Market.APIs.Dtos;

namespace Market.APIs.Helpers
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageSize, int pageIndex,int count, IReadOnlyList<T> data)
        {

            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
