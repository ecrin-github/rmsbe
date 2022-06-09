using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StatisticsApiController : BaseApiController
{
    private readonly IDtpRepository _dtpRepository;
    private readonly IDupRepository _dupRepository;

    public StatisticsApiController(
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
 
    private readonly IObjectRepository _objectRepository;
    private readonly IStudyRepository _studyRepository;

    public StatisticsApiController(
        IObjectRepository objectRepository,
        IStudyRepository studyRepository)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
    }

    [HttpGet("statistics/studies/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetTotalStudies()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _studyRepository.GetTotalStudies()
        });    
    }

    [HttpGet("statistics/data-objects/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetTotalDataObjects()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _objectRepository.GetTotalDataObjects()
        });    
    }
}
