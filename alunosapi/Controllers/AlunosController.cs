using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/xml")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _context;

        public AlunosController(IAlunoService context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest,"Deu ruim")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _context.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter alunos");
            }
        }

        [HttpGet("AlunosPorNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string nome)
        {
            try
            {
                var alunos = await _context.GetAlunosByNome(nome);

                if (alunos.Count() == 0)
                {
                    return NotFound($"Não foi encontrado alunos com o critério {nome}");
                }

                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Inválido");
            }
        }

        [HttpGet("{id:int}", Name = "AlunoPorId")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAluno(int id)
        {
            try
            {
                var aluno = await _context.GetAluno(id);

                if (aluno is null)
                {
                    return NotFound($"Não foi encontrado aluno com o id {id}");
                }

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Inválido");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno)
        {
            try
            {
                await _context.CreateAluno(aluno);
                return CreatedAtRoute("AlunoPorId", new { id = aluno.Id }, aluno);
            }
            catch
            {
                return BadRequest("Inválido");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Aluno aluno)
        {
            try
            {
                if (aluno.Id == id)
                {
                    await _context.UpdateAluno(aluno);
                    return NoContent();
                }

                return BadRequest("Dados inconsistentes");
            }
            catch
            {
                return BadRequest("Inválido");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _context.GetAluno(id);

                if (aluno is not null)
                {
                    await _context.DeleteAluno(aluno);
                    return Ok();
                }
                else
                    return NotFound($"Não foi encontrado aluno com o id {id}");

                return BadRequest("Dados inconsistentes");
            }
            catch
            {
                return BadRequest("Inválido");
            }
        }
    }
}
