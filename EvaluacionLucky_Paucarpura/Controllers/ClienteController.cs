using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace EvaluacionLucky_Paucarpura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClienteController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetAllClientes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var clientes = await connection.QueryAsync<Cliente>("select * from Clientes");
            return Ok(clientes);
        }

        [HttpGet ("{clienteId}")]
        public async Task<ActionResult<Cliente>> GetCliente( int clienteId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cliente = await connection.QueryFirstAsync<Cliente>("select * from Clientes where idCliente = @idCliente",
                new {idCliente = clienteId});
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<List<Cliente>>> CreateCliente(Cliente usuario)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Clientes (nombresCliente, apellidosCliente, correo, telefono) values (@NombresCliente, @ApellidosCliente, @Correo, @Telefono)", usuario);
            return Ok(await SelectAllcliente(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Cliente>>> UpdateCliente(Cliente usuario)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update Clientes set nombresCliente = @NombresCliente, apellidosCliente = @ApellidosCliente, correo = @Correo, telefono = @Telefono where idCliente = @IdCliente", usuario);
            return Ok(await SelectAllcliente(connection));
        }

        [HttpDelete("{clienteId}")]
        public async Task<ActionResult<List<Cliente>>> DeleteCliente(int clienteId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from Clientes where idCliente = @idCliente", new { idCliente = clienteId });
            return Ok(await SelectAllcliente(connection));
        }

        private static async Task<IEnumerable<Cliente>> SelectAllcliente(SqlConnection connection)
        {
            return await connection.QueryAsync<Cliente>("select * from Clientes");
        }
    }
}
