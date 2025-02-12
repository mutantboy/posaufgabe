using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe2.DTOs
{
    public record CreateStudentDto(string FirstName, string LastName);

    public record StudentResponseDto(int Id, string FirstName, string LastName);

    public record CreateTopicDto(string Name, int Seats);

    public record TopicResponseDto(int Id, string Name, int Seats, int EnrolledStudentsCount);

    public record EnrollStudentDto(int StudentId);

    public record EnrollRequestDto(
    int StudentId,
    string FirstChoiceSubject,
    string SecondChoiceSubject,
    string ThirdChoiceSubject
    );
}
