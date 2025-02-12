using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Services;
using FluentValidation;
using Spg.Fachtheorie.Aufgabe2.DTOs;
using Spg.Fachtheorie.Aufgabe2.Model;

namespace Spg.Fachtheorie.Aufgabe3.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly Aufgabe2Database _db;
        private readonly IValidator<CreateTopicDto> _validator;

        public TopicsController(Aufgabe2Database db, IValidator<CreateTopicDto> validator)
        {
            _db = db;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var subject = new Subject(dto.Name, dto.Seats);
            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTopic), new { id = subject.Id }, new TopicResponseDto(subject.Id, subject.Name, subject.Seats, subject.Students.Count));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllTopics()
        {
            var topics = _db.Subjects
                .Include(s => s.Students)
                .Select(s => new TopicResponseDto(s.Id, s.Name, s.Seats, s.Students.Count))
                .ToList();

            return Ok(topics);
        }

        [HttpGet("{id}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEnrolledStudents(int id)
        {
            var subject = _db.Subjects
                .Include(s => s.Students)
                .FirstOrDefault(s => s.Id == id);

            if (subject == null)
                return NotFound();

            var students = subject.Students.Select(s => new StudentResponseDto(s.Id, s.FirstName, s.LastName)).ToList();
            return Ok(students);
        }

        [HttpPost("{id}/enroll")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnrollStudent(int id, [FromBody] EnrollStudentDto dto)
        {
            var subject = await _db.Subjects.Include(s => s.Students).FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
                return NotFound("Topic not found.");

            var student = await _db.Students.FindAsync(dto.StudentId);
            if (student == null)
                return NotFound("Student not found.");

            if (subject.Students.Count >= subject.Seats)
                return BadRequest("No available seats.");

            subject.Students.Add(student);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTopic(int id)
        {
            var subject = _db.Subjects
                .Include(s => s.Students)
                .FirstOrDefault(s => s.Id == id);

            if (subject == null)
                return NotFound();

            return Ok(new TopicResponseDto(subject.Id, subject.Name, subject.Seats, subject.Students.Count));
        }
    }
}