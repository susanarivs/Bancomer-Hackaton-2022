using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OData.SmallVille.Models;
using System.Linq;

namespace OData.SmallVille.Controllers
{
    public class ProspectosController : ODataController
    {
        private SourceStoreContext _db;

        public ProspectosController(SourceStoreContext context)
        {
            _db = context;
        }

        // Devuelve todos los prospectos
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Prospectos);
        }

        // Devuelve un prospecto especifico
        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_db.Prospectos.FirstOrDefault(c => c.Id == key));
        }

        // Crea un nuevo prospecto
        [EnableQuery]
        public IActionResult Post([FromBody] Prospecto prospecto)
        {
            _db.Prospectos.Add(prospecto);
            _db.SaveChanges();
            return Created(prospecto);
        }
    }
}
