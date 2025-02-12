using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Services;
using FluentValidation;
using Spg.Fachtheorie.Aufgabe2.DTOs;
using Spg.Fachtheorie.Aufgabe2.Model;

namespace Spg.Fachtheorie.Aufgabe3.API.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Aufgabe2Database _db;
        private readonly IValidator<CreateStudentDto> _validator;

        public StudentsController(Aufgabe2Database db, IValidator<CreateStudentDto> validator)
        {
            _db = db;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var student = new Student(dto.FirstName, dto.LastName);
            _db.Students.Add(student);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, new StudentResponseDto(student.Id, student.FirstName, student.LastName));
        }

        [HttpGet("{id}/topics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEnrolledTopics(int id)
        {
            var student = _db.Students
                .Include(s => s.Subjects)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound();

            var topics = student.Subjects.Select(s => new TopicResponseDto(s.Id, s.Name, s.Seats, s.Students.Count)).ToList();
            return Ok(topics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudent(int id)
        {
            var student = _db.Students.Find(id);
            if (student == null)
                return NotFound();

            return Ok(new StudentResponseDto(student.Id, student.FirstName, student.LastName));
        }
    }
}