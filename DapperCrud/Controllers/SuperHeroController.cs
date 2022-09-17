using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<SuperHero> heroes = await SelectAllHeroes(connection);
            return Ok(heroes);
        }

        private Task<IEnumerable<SuperHero>> SelectAllHeroes(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{heroId}")]

        public async Task<ActionResult<SuperHero>> GetHero(int heroId) 
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<SuperHero>("select * from superheroes where id = @Id",
                new {Id = heroId});
            return Ok(hero);
        }


        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into superheroes (Name, FirstName, LastName, Location) value (@Name, @FirstName, @LastName, @Location)",hero);
            return Ok(await SelectAllHeroes(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update superheroes set Name= @Name, FirstName=@FirstName, LastName=@LastName, Location=@Location", hero);
            return Ok(await SelectAllHeroes(connection));
        }


        [HttpDelete("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from superheroes where id = @Id",new { Id =heroId });
            return Ok(await SelectAllHeroes(connection));
        }

    }
}
