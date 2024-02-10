using AuctionService.Domain.DTOs;
using AuctionService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionServices _services;

        public AuctionsController(IAuctionServices services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetbyId(Guid id)
        {
            var auction = await _services.GetById(id);

            if (auction is not null)
                return auction;

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAll(string date)
        {
            var auctions = await _services.GetAll(date);

            if (auctions is not null)
                return auctions;

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            string seller = User.Identity.Name;

            var result = await _services.Create(auctionDto, seller);

            if (result is not null)
                return CreatedAtAction(nameof(GetbyId), new { result.Id }, result);

            return BadRequest("Could not save changes to the DB");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            string seller = User.Identity.Name;

            var result = await _services.Update(id, updateAuctionDto, seller);

            if (result is null) return Forbid();

            if (result is not null) return Ok();

            return BadRequest("Problem saving changes");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            string seller = User.Identity.Name;

            var wasDeleted = await _services.DeleteById(id, seller);

            if (wasDeleted)
                return Ok(wasDeleted);

            return BadRequest("Problem deleting auction");
        }
    }
}
