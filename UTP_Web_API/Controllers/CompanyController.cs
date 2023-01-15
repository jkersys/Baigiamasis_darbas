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
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ICompanyRepository companyRepo, ICompanyAdapter companyAdapter, ILogger<CompanyController> logger)
        {
            _companyRepo = companyRepo;
            _companyAdapter = companyAdapter;
            _logger = logger;
        }

        /// <summary>
        /// gaunamas imoniu sarasas, kuris naudojamas front ende selectui
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("companies/list/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyResponseForFrondEndDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompaniesForFe()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get companies");
                var companies = await _companyRepo.GetAllAsync();

                if (companies == null)
                {
                    _logger.LogInformation($"{DateTime.Now} companies not found");
                    return NotFound();
                }
                return Ok(companies
                .Select(c => new CompanyResponseForFrondEndDto(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetCompaniesForFe exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gaunama viena imone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("company/{id:int}", Name = "GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCompanyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetCompanyDto>> GetCompanyById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get company with id {id} ");
                if (id == 0)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} not valid");
                    return BadRequest();
                }
                var company = await _companyRepo.GetAsync(x => x.CompanyId == id);

                if (company == null)
                {
                    _logger.LogInformation($"{DateTime.Now}company {id} not found");
                    return NotFound();
                }

                var companyDto = _companyAdapter.Bind(company);

                return Ok(companyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetCompanyById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// grazina visa imoniu sarasa
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("companies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCompanyDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get companies");
                var companies = await _companyRepo.GetAllAsync();

                if (companies == null)
                {
                    _logger.LogInformation($"{DateTime.Now} companies not found");
                    return NotFound();
                }
                return Ok(companies
                .Select(c => _companyAdapter.Bind(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetCompanies exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// sukuriama nauja imone
        /// </summary>
        /// <param name="companyDto"></param>
        /// <returns></returns>
        ///<response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("company")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetCompanyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCompanyDto>> CreateCompany(CreateCompanyDto companyDto)
        {
            try
            {
                _logger.LogError($"{DateTime.Now} attempt to create new company.");
                if (companyDto == null)
                {
                    _logger.LogError($"{DateTime.Now} input {companyDto} not valid");
                    return BadRequest();
                }

                var createCompany = _companyAdapter.Bind(companyDto);
                var isExist = await _companyRepo.ExistAsync(x => x.CompanyRegistrationNumber == companyDto.CompanyRegistrationNumber);

                if (isExist == true)
                {
                    _logger.LogError($"{DateTime.Now} company with {companyDto.CompanyRegistrationNumber} already exist");
                    return BadRequest("Imone jau yra duomenu bazeje");
                }

                await _companyRepo.CreateAsync(createCompany);
                return CreatedAtRoute("GetCompany", new { id = createCompany.CompanyId }, _companyAdapter.Bind(createCompany));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateConclusion exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// atnaujinami imones duomenys
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCompanyDto"></param>
        /// <returns></returns>
        [HttpPut("companies/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCompany(int id, CreateCompanyDto updateCompanyDto)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to update conclusion {id}");
                if (id == 0 || updateCompanyDto == null)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} or {updateCompanyDto} not valid");
                    return BadRequest();
                }

                var foundCompany = await _companyRepo.GetAsync(c => c.CompanyId == id);
                if (foundCompany == null)
                {
                    _logger.LogInformation($"{DateTime.Now} company Nr. {id} not exist");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} UpdateCompany exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Istrinama imone is duomenu bazes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("companies/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to delete company id {id}.");
                if (id == 0)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} not valid.");
                    return BadRequest();
                }

                var company = await _companyRepo.GetAsync(d => d.CompanyId == id);

                if (company == null)
                {
                    _logger.LogInformation($"{DateTime.Now} company id Nr. {id} not found");
                    return NotFound();
                }

                await _companyRepo.RemoveAsync(company);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteCompany exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
