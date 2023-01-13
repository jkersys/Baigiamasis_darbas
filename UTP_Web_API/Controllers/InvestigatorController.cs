﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigatorDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigatorController : ControllerBase
    {
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly IInvestigatorAdapter _iAdapter;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<InvestigatorController> _logger;

        public InvestigatorController(IInvestigatorRepository investigatorRepo, IInvestigatorAdapter iAdapter, IUserRepository userRpo, ILogger<InvestigatorController> logger)
        {
            _investigatorRepo = investigatorRepo;
            _iAdapter = iAdapter;
            _userRepo = userRpo;
            _logger = logger;
        }
        /// <summary>
        /// sukuria nauja tyreja sujungiant investigator su localuser ir pakeicia role i investigator
        /// </summary>
        /// <param name="investigator">pazymejimo nr, kabineto nr, darbovietes adr.,vartotojo el. pastas</param>
        /// <returns></returns>     
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("investigator")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<GetInvestigatorDto>> CreateInvestigator(CreateInvestigatorDto investigator)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} was atempted to create new investigator");

                if (investigator == null)
                {
                    return BadRequest();
                }
                var user = await _userRepo.GetUser(investigator.VartotojoElPastas);
                if (user == null)
                {
                    _logger.LogInformation($"{DateTime.Now} User with {investigator.VartotojoElPastas} email was not found");
                    return NotFound($"User email {investigator.VartotojoElPastas} not found");
                }
                if (user.Investigator != null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigator with {investigator.VartotojoElPastas} already exist");
                    return BadRequest("Tyrejas tokiu vardu jau yra");
                }
                var createInvestigator = _iAdapter.Bind(investigator, user);
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), "Investigator");
                await _investigatorRepo.CreateAsync(createInvestigator);

                return CreatedAtRoute("GetInvestigator", new { id = createInvestigator.InvestigatorId }, _iAdapter.Bind(createInvestigator));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// grazina viena tyreja su pilnais jo duomenimis
        /// </summary>
        /// <param name="id">id number</param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("investigator/{id:int}", Name = "GetInvestigator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetInvestigatorDto>> InvestigatorById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get investigator with id {id} ");
            if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} Input {id} is not valid");
                    return BadRequest();
            }
            var investigator = await _investigatorRepo.GetById(id);

            if (investigator == null)
            {
                _logger.LogInformation($"{DateTime.Now} Investigator with id {id} not exist");
                return NotFound();
            }
            return Ok(_iAdapter.Bind(investigator));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} InvestigatorById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Ištrina tyrėja iš duomenu bazės, jeigu jis neturi bylu.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("investigator/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteInvestigator(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to delete investigator with id {id} ");
                if (id == 0)
                {
                    return BadRequest();
                }
                var investigator = await _investigatorRepo.GetById(id);

                if (investigator == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigator with id {id} was not found");
                    return NotFound();
                }

                if (investigator.Complains.Count() != 0 || investigator.AdministrativeInspections.Count != 0 || investigator.Investigations.Count() != 0)
                {
                    _logger.LogWarning($"{DateTime.Now} Atempted to delete investigator Nr. {id}, this investigator have cases");
                    return BadRequest("Investigator have cases");
                }
                await _investigatorRepo.RemoveAsync(investigator);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
