using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Models.Dto.InvestigationDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigationController : ControllerBase
    {
        private readonly IInvestigationRepository _investigatonRepo;
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IInvestigationAdapter _investigationAdapter;

        public InvestigationController(IInvestigationRepository investigatonRepo, IInvestigatorRepository investigatorRepo, ICompanyRepository companyRepo, IInvestigationAdapter investigationAdapter)
        {
            _investigatonRepo = investigatonRepo;
            _investigatorRepo = investigatorRepo;
            _companyRepo = companyRepo;
            _investigationAdapter = investigationAdapter;
        }

        //[Authorize(Roles = "Customer")]
        [HttpGet("investigation/{id:int}", Name = "GetInvestigation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOneInvestigationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetOneInvestigationDto>> GetInvestigationById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            if (await _investigatonRepo.ExistAsync(x => x.InvestigationId == id) == false)
            {
                return NotFound();
            }
            var complain = await _investigatonRepo.GetById(id);

            var complainDto = _investigationAdapter.BindForOneInvestigation(complain);

            return Ok(complainDto);
        }

        [HttpGet("investigations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInvestigationsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInvestigations()
        {
            var investigations = await _investigatonRepo.All();

            if (investigations == null)
            {
                return NotFound();
            }
            return Ok(investigations
            .Select(c => _investigationAdapter.Bind(c))
            .ToList());
        }

        [HttpPut("investigation/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateInvestigation(int id, UpdateInvestigationDto updateComplainDto)
        {
            if (updateComplainDto == null)
            {
                return BadRequest();
            }
            if (await _investigatonRepo.ExistAsync(x => x.InvestigationId == id) == false)
            {
                return NotFound();
            }
            var foundCompany = await _companyRepo.GetAsync(i => i.CompanyId == updateComplainDto.CompanyId);
            var foundInvestigation = await _investigatonRepo.GetById(id);
            var foundInvestigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == updateComplainDto.InvestigatorId);
            var stage = new InvestigationStage
            {
                Stage = updateComplainDto.InvestigationStage,
                TimeStamp = DateTime.Now,
            };
            foundInvestigation.Company = foundCompany;
            foundInvestigation.Investigators?.Add(foundInvestigator);
            foundInvestigation.Stages?.Add(stage);

            await _investigatonRepo.Update(foundInvestigation);

            return NoContent();

        }


        [HttpPost("investigation")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateInvestigationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateInvestigationDto>> CreateInvestigation(CreateInvestigationDto investigation)
        {
            if (investigation == null)
            {
                return BadRequest();
            }            
            var foundInvestigator = await _investigatorRepo.GetAsync(c => c.InvestigatorId == investigation.InvestigatorId);
            var foundCompany = await _companyRepo.GetAsync(i => i.CompanyId == investigation.CompanyId);
            var stage = new InvestigationStage
            {
                Stage = investigation.InvestigationStage,
                TimeStamp = DateTime.Now,
            };
            if (foundInvestigator == null || foundCompany == null)
            {
                return BadRequest();
            }

            var newInvestigation = new Investigation();
            newInvestigation.Company = foundCompany;
            newInvestigation.LegalBase = investigation.LegalBase;
            newInvestigation.StartDate = DateTime.Now;
            newInvestigation.Investigators?.Add(foundInvestigator);           
            newInvestigation.Stages?.Add(stage);           

            await _investigatonRepo.CreateAsync(newInvestigation);
            return Ok();
        }

        [HttpDelete("investigation/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteInvestigation(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var investigation = await _investigatonRepo.GetAsync(d => d.InvestigationId == id);

            if (investigation == null)
            {
                return NotFound();
            }

            await _investigatonRepo.RemoveAsync(investigation);

            return NoContent();
        }

    }
}
    

