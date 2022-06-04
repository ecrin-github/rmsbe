namespace rmsbe.Contracts
{
    public class FilteringByTitleRequest : PaginationRequest
    {
        public string Title { get; set; }
    }
}