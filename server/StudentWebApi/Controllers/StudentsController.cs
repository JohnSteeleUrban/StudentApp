using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentWebApi.Models;
using StudentWebApi.Repository;

namespace StudentWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string sortBy, int order, int page, int pageSize)
        {
            var students = await _studentRepository.GetAllStudents(sortBy, (SortOrder)order, page, pageSize);

            var paginationMetadata = new
            {
                totalCount = students.TotalCount,
                pageSize = pageSize,
                currentPage = page
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return new ObjectResult(students);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var student = await _studentRepository.GetStudent(id);

            if (student == null)
                return new NotFoundResult();

            return new ObjectResult(student);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Student student)
        {
            await _studentRepository.Create(student);
            return new OkObjectResult(student);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Student student)
        {
            var studentFromDb = await _studentRepository.GetStudent(student.Id);

            if (studentFromDb == null)
                return new NotFoundResult();

            await _studentRepository.Update(student);

            return new OkObjectResult(student);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var studentFromDb = await _studentRepository.GetStudent(id);

            if (studentFromDb == null)
                return new NotFoundResult();

            await _studentRepository.Delete(id);

            return new OkResult();
        }
    }
}
