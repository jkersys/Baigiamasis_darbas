using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainStageController : ControllerBase
    {
        private readonly IComplainRepository _complainRepo;
        
        public ComplainStageController(IComplainRepository complainRepo)
        {
            _complainRepo = complainRepo;           
        }
       
        [HttpPut("investigator/complains/stage/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddStagesToComplain(int id, AddNewStage stage)
        {
            if (stage == null)
            {
                return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                return NotFound();
            }
            var foundComplain = await _complainRepo.GetAsync(x => x.ComplainId == id);       
                        
            var newStage = new InvestigationStage { Stage = stage.Stage, TimeStamp = DateTime.Now };
          
            foundComplain.Stages?.Add(newStage);

            await _complainRepo.Update(foundComplain);

            return NoContent();

        }

    }
}
