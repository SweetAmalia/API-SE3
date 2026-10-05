using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    [HttpGet]   
    public string GetString()
    {
        return "Het endpoint werkt!";
    }

    [HttpGet("GetStudent")]
    public Student GetStudent()
    {
        Student student = new Student(25012345L, "Jelle");
        return student;
    }

    [HttpGet("studentmetstatus")]
    public ActionResult<Student> GetStudentMetStatus()
    {
        Student student = new Student(25012345L, "Jelle");
        if (student.Naam == "Jelle")
        {
            return NotFound("Student niet gevonden");
        }
        return student;
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetStudentById(int id)
    {
        var student = Student.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        return student;
    }

    [HttpGet("async/{id}")]
    public async Task<ActionResult<Student>> GetStudentByIdAsync(int id)
    {
        await Task.Delay(10);
        var student = Student.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        return student;
    }
}