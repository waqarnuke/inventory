using API.Dtos.UserLocationAssignment;
using API.Errors;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserLocationAssignmentController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLocationAssignmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("assignLocation")]
        public async Task<IActionResult> AssignLocation([FromBody] UserLocationAssignmentDto dto)
        {
            var exists = await _unitOfWork.userLocationAssignmentsRepository.Get(x => x.UserId == dto.UserId && x.LocationId == dto.LocationId);

            // await _context.UserLocationAssignments
            // .AnyAsync(x => x.UserId == dto.UserId && x.LocationId == dto.LocationId);

            if (exists != null)
                return BadRequest("User already assigned to this location");

            var assignment = new UserLocationAssignment
            {
                UserId = dto.UserId ?? "xxxx",
                LocationId = dto.LocationId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedById = dto.CreatedById,
                CreatedUser = dto.FirstName + " " + dto.LastName,
                CreatedTime = DateTime.UtcNow,
            };

            _unitOfWork.userLocationAssignmentsRepository.Add(assignment);

            var result = await _unitOfWork.Save();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem assigning location"));

            return Ok();

        }

        [HttpPut("updateassignLocation")]
        public async Task<IActionResult> updateassignLocation([FromBody] UserLocationAssignmentDto dto)
        {
            var exists = await _unitOfWork.userLocationAssignmentsRepository.Get(x => x.Id == dto.Id);

            if (exists == null) return NotFound(new ApiResponse(404, "User location assignment not found"));

            exists.LocationId = dto.LocationId;
            exists.UpdatedTime = DateTime.UtcNow;
            exists.UpdatedUser = dto.FirstName + " " + dto.LastName;

            _unitOfWork.userLocationAssignmentsRepository.Update(exists);

            var result = await _unitOfWork.Save();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating  location assignment."));

            return Ok(result);

        }

        [HttpGet("getLocations")]
        public async Task<IActionResult> GetLocations(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var locationIds = await _unitOfWork.userLocationAssignmentsRepository.GetAllById(x => x.UserId == userId);

            var selected = locationIds.Select(x => x.LocationId).ToList();

            var locations = await _unitOfWork.locationRepository.GetAllById(x => selected.Contains(x.Id));

            return Ok(locations);
        }

        [HttpGet("getUserLocationsByAdmin")]
        public async Task<IActionResult> GetUserLocationsByAdmin(string createdById)
        {
            if (string.IsNullOrEmpty(createdById)) return NotFound();

            var locationIds = await _unitOfWork.userLocationAssignmentsRepository.GetAllById(x => x.UserId == createdById);

            var selected = locationIds.Select(x => x.LocationId).ToList();

            var locations = await _unitOfWork.locationRepository.GetAllById(x => selected.Contains(x.Id));

            return Ok(locations);
        }

        [HttpGet("getAssignedLocations")]
        public async Task<IActionResult> GetAssignedLocations(string createdById)
        {
            if (string.IsNullOrEmpty(createdById)) return NotFound();

            var assignedLocation = await _unitOfWork.userLocationAssignmentsRepository.GetAllById(x => x.CreatedById == createdById);

            if (assignedLocation == null || !assignedLocation.Any()) return NotFound(new ApiResponse(404));

            return Ok(assignedLocation);
        }

        [HttpDelete("deleteLocationAssignment")]
        public async Task<IActionResult> DeleteLocationAssignment(int id)
        {
            var locationAssignment = await _unitOfWork.userLocationAssignmentsRepository.Get(x => x.Id == id);

            if (locationAssignment == null) return NotFound(new ApiResponse(404));

            _unitOfWork.userLocationAssignmentsRepository.Remove(locationAssignment);

            var result = await _unitOfWork.Save();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting location assignment"));

            return Ok(result);
        }
    }
}
