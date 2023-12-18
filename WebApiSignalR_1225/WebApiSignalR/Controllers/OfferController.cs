using Microsoft.AspNetCore.Mvc;
using WebApiSignalR.Helpers;
using WebApiSignalR.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IRoomRepository _repository;

        public OfferController(IRoomRepository repository)
        {
            _repository = repository;
            var rooms = _repository!.Getall();
            foreach (var room in rooms)
            {
                if (!System.IO.File.Exists(room.Name + ".txt"))
                {
                    FileHelper.Write(room.Name, room.Price);
                }
            }
        }

        // GET: api/<OfferController>
        [HttpGet]
        public double Get()
        {
            return FileHelper.Read();
        }

        [HttpGet("/Room")]
        public double Get(string room)
        {
            return FileHelper.Read(room);
        }

        // GET: api/<OfferController>
        [HttpGet("/Increase")]
        public void Increase(double data)
        {
            var result=FileHelper.Read()+data;
            FileHelper.Write(result);   
        }

        // GET: api/<OfferController>
        [HttpGet("/IncreaseRoom")]
        public void Increase(string room,double number)
        {
            var result = FileHelper.Read(room) + number;
            FileHelper.Write(room,result);
            var uproom=_repository.Get(room);
            uproom.Price = result;
            _repository.Update(uproom);
        }

        // GET api/<OfferController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OfferController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OfferController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OfferController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
