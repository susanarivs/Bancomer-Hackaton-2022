using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OData.SmallVille.Models;
using System.Linq;

namespace OData.SmallVille.Controllers
{
    public class CanalesProspectosController : ODataController
    {
        private SourceStoreContext _db;

        public CanalesProspectosController(SourceStoreContext context)
        {
            _db = context;
        }

        // Devuelve todos los canales-prospectos
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.CanalesProspectos);
        }

        // Crea un nuevo canal-prospecto
        [EnableQuery]
        public IActionResult Post([FromBody] CanalProspecto canalProspecto)
        {
            _db.CanalesProspectos.Add(canalProspecto);
            _db.SaveChanges();
            return Created(canalProspecto);
        }
    }
}
