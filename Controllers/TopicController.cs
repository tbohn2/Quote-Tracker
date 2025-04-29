using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Quote_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly Quote_Tracker_Context _context;

        public TopicController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var Topics = await _context.Topics.ToListAsync();

            if (Topics.Count == 0)
            {
                return Ok("No Topics found");
            }

            return Ok(Topics);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid format; check for missing information (Topic or book source).");
            }

            var newTopic = new Topic
            {
                Name = request.Name
            };

            _context.Topics.Add(newTopic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllTopics), new { id = newTopic.Id }, newTopic);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopicRequest updatedTopic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state not valid");
            }

            var topicToUpdate = await _context.Topics.FindAsync(updatedTopic.Id);
            if (topicToUpdate == null)
            {
                return NotFound("Topic not found.");
            }

            topicToUpdate.Name = updatedTopic.Name;

            _context.Topics.Update(topicToUpdate);
            await _context.SaveChangesAsync();

            return Ok(topicToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Topic ID.");
            }

            var TopicToDelete = await _context.Topics.FindAsync(id);

            if (TopicToDelete == null)
            {
                return NotFound($"No Topic found with ID {id}");
            }

            _context.Topics.Remove(TopicToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
