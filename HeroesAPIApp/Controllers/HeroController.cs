using HeroesAPIApp.Data;
using HeroesAPIApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SuperHeroWebAPI.Tools;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeroesAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HeroController : ControllerBase
    {
        readonly string url = "https://www.superheroapi.com/api.php/10220849299613198/";

        public readonly DataContext _context;

        public HeroController(DataContext context)
        {

            _context = context;
        }

        public static async Task<Tuple<String, String>> checkStatusAsync(HttpResponseMessage response)
        {
            var status = response.StatusCode.ToString();
            switch (status)
            {

                case "OK":
                    Console.WriteLine("OK");
                    var content = await response.Content.ReadAsStringAsync();
                    return new Tuple<String, String>("200", content);
                    break;

                case "NotFound":
                    Console.WriteLine("Not Found");
                    return new Tuple<String, String>("404", "Not Found");

                    break;

                case "Unauthorized":
                    Console.WriteLine("Unauthorized");
                    return new Tuple<String, String>("401", "----------Unauthorized----------------");
                    break;
                case "BadRequest":
                    Console.WriteLine("Bad Request");


                    return new Tuple<String, String>("400", "Bad Request");

                case "NoContent":
                    Console.WriteLine("No Content");


                    return new Tuple<String, String>("204", "No Content");


                default:
                    Console.WriteLine("autre erreur système");
                    return new Tuple<String, String>(response.StatusCode.ToString(), "autre erreur système");
                    break;
            }


        }

        private Rootobject JsonResponseHero(int id, out Rootobject new_clas_result)
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };



            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Log.WriteLine("THIS IS A TEST STARTING HERE");
                var new_url = url + "" + id;
                var response = httpClient.GetAsync(new_url).Result;
                var stringResult = response.Content.ReadAsStringAsync().Result;

                var resultat = JsonConvert.DeserializeObject<Rootobject>(stringResult);

                new_clas_result = resultat;
                Console.WriteLine(resultat.ToString());
               

                return new_clas_result;
            }
        }


        // GET api/<HeroController>/5
        [HttpGet("{id}") , Authorize]
        public Rootobject GetHero(int id){
            Rootobject new_clas_result = new Rootobject();
            try
            {
                return JsonResponseHero(id, out new_clas_result);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
                throw;
            }
        }



        [HttpGet , AllowAnonymous]

        public async Task<ActionResult<List<Hero>>> GetMeAllHeroes()
        {
            return await _context.Heroes.ToListAsync();
        }

        
        //// GET api/CreatingHeroes/
        //[HttpGet]
        //public async Task<ActionResult<List<Rootobject>>> CreateHeroes()
        //{

        //    for (int i = 151; i < 732; i++)
        //    {
        //        Rootobject new_clas_result = new Rootobject();
        //        try
        //        {
        //            new_clas_result =  JsonResponseHero(i, out new_clas_result);
        //            Hero hero = new Hero
        //            {
        //                Name = new_clas_result.name,
        //                ImageUrl = new_clas_result.image.url
        //            };
        //            _context.Heroes.Add(hero);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }

        //    }
        //    await _context.SaveChangesAsync();
        //    return Ok(await _context.Heroes.ToListAsync());
        //}

        //GET api/GetMeAllHeroes

        //Allow Anyone to get list with AllowAnonymous
        //
        //// POST api/<HeroController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<HeroController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<HeroController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
