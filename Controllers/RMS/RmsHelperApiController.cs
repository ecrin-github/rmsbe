using Microsoft.AspNetCore.Mvc;
using rmsbe.DataLayer.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class RMSStatisticsApiController : BaseApiController
{
    /*
    private readonly IDtpRepository _dtpRepository;
    private readonly IDupRepository _dupRepository;

    public RMSStatisticsApiController(
        IDtpRepository dtpRepository,
        IDupRepository dupRepository)
    {
        _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        _dupRepository = dupRepository ?? throw new ArgumentNullException(nameof(dupRepository));
    }

    [HttpGet("statistics/dtp/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetDtpStatistics()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _dtpRepository.GetTotalDtp(),
            Uncompleted = await _dtpRepository.GetUncompletedDtp()
        });    
    }

    [HttpGet("statistics/dup/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetDupStatistics()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _dupRepository.GetTotalDup(),
            Uncompleted = await _dupRepository.GetUncompletedDup()
        });    
    }
    
    
    [HttpPost("pagination/dtp")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateStudies(PaginationRequest paginationRequest)
    {
        var data = await _dtpRepository.PaginateDtp(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<Dtp>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DTPs have been found."}
        });
        return Ok(new ApiResponse<Dtp>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("pagination/dup")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateObjects(PaginationRequest paginationRequest)
    {
        var data = await _dupRepository.PaginateDup(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<Dup>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DUPs have been found."}
        });
        return Ok(new ApiResponse<Dup>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("filter/dtp/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _dtpRepository.FilterDtpByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<Dtp>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DTPs have been found."}
        });
        return Ok(new ApiResponse<Dtp>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    
    [HttpPost("filter/dup/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterDupByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _dupRepository.FilterDupByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<Dup>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DUPs have been found."}
        });
        return Ok(new ApiResponse<Dup>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
*/

}
