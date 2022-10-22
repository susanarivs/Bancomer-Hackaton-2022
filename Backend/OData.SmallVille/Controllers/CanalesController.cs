using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OData.SmallVille.Models;
using System.Linq;

namespace OData.SmallVille.Controllers
{
    public class CanalesController : ODataController
    {
        private SourceStoreContext _db;

        public CanalesController(SourceStoreContext context)
        {
            _db = context;
        }

        // Devuelve todos los canales
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Canales);
        }

        // Devuelve un canal especifico
        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_db.Canales.FirstOrDefault(c => c.Id == key));
        }

        // Crea un nuevo canal
        [EnableQuery]
        public IActionResult Post([FromBody] Canal canal)
        {
            _db.Canales.Add(canal);
            _db.SaveChanges();
            return Created(canal);
        }
    }
}
