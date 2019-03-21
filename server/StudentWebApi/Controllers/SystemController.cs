using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentWebApi.Models;
using StudentWebApi.Repository;

namespace StudentWebApi.Controllers
{

    //this class is sloppy and done quickly just to update seed data and login.
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOptions<Settings> _settings;
        private IEnumerable<UiNamesApi> _personList;

        public SystemController(IStudentRepository studentRepository, IOptions<Settings> settings)
        {
            _studentRepository = studentRepository;
            _settings = settings;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login(string userName, string password)
        {
            if (userName == null || password == null)
            {
                return BadRequest("Invalid client request");
            }

            if (userName == "admin" && password == "adminPa$$word987")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.SuperSecretDontTellNobody));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(100),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet, Route("seedDatabase/{id}")]
        public IActionResult SeedDatabase(int id)
        {
            //id must be between 1-500.  That is what the uinames api can handle in one batch.
            if (id < 0 || id > 500)
            {
                return BadRequest("please choose an int between 1-500.");
            }

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://uinames.com/api");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //500 is the limit
            var response = client.GetAsync(string.Format("?amount={0}", id)).Result;
            if (response.IsSuccessStatusCode)
            {
                _personList = response.Content.ReadAsAsync<IEnumerable<UiNamesApi>>().Result;
                foreach (var person in _personList)
                {
                    _studentRepository.Create(new Student()
                    {
                        Email = person.Name + "." + person.Surname + "@email.com",
                        FirstName = person.Name,
                        LastName = person.Surname,
                        LastUpdated = DateTime.UtcNow,
                    });
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);//TODO: create some sort of logging because this falls into the ether.
            }

            return Ok(new { Result = string.Format("database seeded with {0} entries.", _personList.Count()) });
        }

        [HttpGet, Route("seedDatabase")]
        public IActionResult SeedDatabase()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://uinames.com/api");

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            for (var i = 0; i < 10; i++)
            {
                //500 is the limit
                var response = client.GetAsync("?amount=500").Result;
                if (response.IsSuccessStatusCode)
                {
                    _personList = response.Content.ReadAsAsync<IEnumerable<UiNamesApi>>().Result;
                    foreach (var person in _personList)
                    {
                        _studentRepository.Create(new Student()
                        {
                            Email = person.Name + "." + person.Surname + "@email.com",
                            FirstName = person.Name,
                            LastName = person.Surname,
                            LastUpdated = DateTime.UtcNow,
                        });
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);//TODO: create some sort of logging because this falls into the ether.
                }
            }

            return Ok(new { Result = string.Format("database seeded with {0} entries.", _personList.Count()) });
        }

        [HttpGet, Route("emptyDatabase")]
        public IActionResult EmptyDatabase()
        {
            return Ok(new { DeleteCount = _studentRepository.DeleteAllData().Result });
        }

        [HttpGet, Route("studentCount")]
        public IActionResult StudentCount()
        {
            return Ok(new { count = _studentRepository.GetStudentCount().Result });
        }

    }
}