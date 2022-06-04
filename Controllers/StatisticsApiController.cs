using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts;
using RmsService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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
    }
