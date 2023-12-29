﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controller
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationsDbContext context;
        public LibroController(ApplicationsDbContext context)
        {
            this.context = context;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libro.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);

        }
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor) return BadRequest($"No existe el autor de Id: {libro.AutorId}");
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
