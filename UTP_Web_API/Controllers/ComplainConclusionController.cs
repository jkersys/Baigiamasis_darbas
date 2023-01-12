using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models.Dto.ConclusionDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainConclusionController : ControllerBase
    {
        private readonly IComplainAdapter _complainAdapter;
        private readonly IConclusionRepository _conclusionRepo;
        private readonly IComplainRepository _complainRepo;
        public ComplainConclusionController(IComplainRepository complainRepo, IComplainAdapter complainAdapter,
               IConclusionRepository conclusionRepo)
        {
            _complainRepo = complainRepo;
            _complainAdapter = complainAdapter;
            _conclusionRepo = conclusionRepo;
        }
        [HttpPut("investigator/complains/conclusion/update/{complainId:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddConclusionToComplain(int complainId, AddConclusionToCaseDto conclusion)
        {
            if (conclusion == null)
            {
                return BadRequest();
            }
           
            var foundComplain = await _complainRepo.GetById(complainId);
            var foundConclusion = await _conclusionRepo.GetAsync(i => i.ConclusionId == conclusion.ConclusionId);

            if (foundComplain == null || foundConclusion == null)
            {
                return NotFound();
            }

            if (foundComplain.Conclusion != null)
            {
                return BadRequest("Byla jau užbaigta");
            }

            foundComplain.Conclusion = foundConclusion;           
            foundComplain.EndDate = DateTime.Now;
           
            await _complainRepo.Update(foundComplain);

            return NoContent();

        }
                
    }
}
