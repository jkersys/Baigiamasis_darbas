using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models.Dto.CompanyDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {  
        private readonly ICompanyRepository _companyRepo;
        private readonly ICompanyAdapter _companyAdapter;
        public CompanyController(ICompanyRepository companyRepo, ICompanyAdapter companyAdapter)
        {
            _companyRepo = companyRepo;
            _companyAdapter = companyAdapter;
        }
        [HttpGet("company/{id:int}", Name = "GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCompanyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetCompanyDto>> GetCompanyById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var company = await _companyRepo.GetAsync(x => x.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }

            var companyDto = _companyAdapter.Bind(company);

            return Ok(companyDto);
        }

        [HttpGet("companies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCompanyDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComplains()
        {
            var companies = await _companyRepo.GetAllAsync();

            if (companies == null)
            {
                return NotFound();
            }
            return Ok(companies
            .Select(c => _companyAdapter.Bind(c))
            .ToList());
        }

        [HttpPost("company")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCompanyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateCompanyDto>> CreateCompany(CreateCompanyDto companyDto)
        {       

            if (companyDto == null)
            {
                return BadRequest();
            }

            var createCompany = _companyAdapter.Bind(companyDto);
            var isExist = await _companyRepo.ExistAsync(x => x.CompanyRegistrationNumber == companyDto.CompanyRegistrationNumber);

            if(isExist == true)
            {
                return BadRequest("Imone jau yra duomenu bazeje");
            }

            await _companyRepo.CreateAsync(createCompany);

            return CreatedAtRoute("GetCompany", new { id = createCompany.CompanyId }, companyDto);

        }

        [HttpPut("companies/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddInvestigatorToComplain(int id, CreateCompanyDto updateCompanyDto)
        {
            if (updateCompanyDto == null)
            {
                return BadRequest();
            }
           
            var foundCompany = await _companyRepo.GetAsync(c => c.CompanyId == id);
            if (foundCompany == null)
            {
                return BadRequest();
            }

            foundCompany.CompanyName = updateCompanyDto.CompanyName;
            foundCompany.CompanyRegistrationNumber = updateCompanyDto.CompanyRegistrationNumber;
            foundCompany.CompanyPhone = updateCompanyDto.CompanyPhone;
            foundCompany.CompanyAdress = updateCompanyDto.CompanyAdress;
            foundCompany.CompanyEmail = updateCompanyDto.CompanyEmail;
            

            await _companyRepo.Update(foundCompany);

            return NoContent();
        }


        [HttpDelete("companies/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var company = await _companyRepo.GetAsync(d => d.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }

            await _companyRepo.RemoveAsync(company);

            return NoContent();
        }

    }
}
