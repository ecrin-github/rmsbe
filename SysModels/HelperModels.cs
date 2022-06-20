using System.Collections.Generic;

namespace rmsbe.SysModels;

public class ApiResponse<T>
{
    public int Total { get; set; }
    public int? Size { get; set; }
    public int? Page { get; set; }
    public int StatusCode { get; set; }
    public IList<string>? Messages { get; set; }
    public ICollection<T>? Data { get; set; }
}

public class EmptyApiResponse
{
    public int Total { get; set; }
    public int? Size { get; set; }
    public int? Page { get; set; }
    public int StatusCode { get; set; }
    public IList<string>? Messages { get; set; }
}

public class PaginationRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class PaginationResponse<T>
{
    public int Total { get; set; }
    public ICollection<T>? Data { get; set; }
}

public class FilteringByTitleRequest : PaginationRequest
{
    public string? Title { get; set; }
}

public class SearchByTitleRequest
{
    public string? OrganisationName { get; set; }
}

public class Statistic
{
    public string? StatType {get; set;}
    public int? StatValue {get; set;}
    
    public Statistic() { }
    
    public Statistic(string statType, int? statValue)
    {
        StatType = statType;
        StatValue = statValue;
    }
}


