using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace web_api.Controllers
{
    public class VeiculosController : ApiController
    {
        private Repositories.IRepository<Models.Veiculo> repository;

        public VeiculosController()
        {
            try
            {
                repository = new Repositories.Database.SQLServer.ADO.Veiculo(Configurations.SQLServer.getConnectionString());
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
            }
        }
        // GET: api/Veiculos
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(repository.get());
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }
        }
        // GET: api/Veiculos/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                Models.Veiculo veiculo = repository.getById(id);

                if (veiculo == null) return NotFound();

                return Ok(veiculo);
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }
        }

        // POST: api/Veiculos
        public IHttpActionResult Post(Models.Veiculo veiculo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                repository.add(veiculo);

                return Ok(veiculo);
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }
        }

        // PUT: api/Veiculos/5
        public IHttpActionResult Put(int id, Models.Veiculo veiculo)
        {
            try
            {
                if (id != veiculo.Id)
                    ModelState.AddModelError("Id", "Id do veículo inválido ou inexistente nos registros");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                int rowsAffected = repository.update(id, veiculo);

                if (rowsAffected == 0) return NotFound();

                return Ok(veiculo);
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }
        }

        // DELETE: api/Veiculos/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                int rowsAffected = repository.delete(id);

                if (rowsAffected == 0) return NotFound();

                return Ok("Veículo excluído do registro!");
            }
            catch (Exception ex)
            {
                Logger.Log.fileWritter(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }

        }
    }
}
