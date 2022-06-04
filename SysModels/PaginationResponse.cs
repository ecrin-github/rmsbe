using System.Collections.Generic;

namespace rmsbe.Contracts
{
    public class PaginationResponse<T>
    {
        public int Total { get; set; }
        public ICollection<T> Data { get; set; }
    }
}