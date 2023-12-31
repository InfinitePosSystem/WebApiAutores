﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationsDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationsDbContext context, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }
        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter (typeof(MiFiltroDeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new {
                AutoresControllerTrasient = servicioTransient.Guid,
                AutoresControllerScoped = servicioScoped.Guid,
                AutoresControllerSingleton = servicioSingleton.Guid,
                ServicioA_Transient = servicio.ObtenerTrasient(),
                ServicioA_Scoped = servicio.ObtenerScoped(),
                ServicioA_Singleton = servicio.ObtenerSingleton()
            });
        }
        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            throw new NotImplementedException();
            logger.LogInformation("Estamos obteniendo los autores");
            servicio.HacerTarea();
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }
        [HttpGet("Primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null) return NotFound();
            return autor;
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null) return NotFound();
            return autor;
        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var existeAutorConMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);
            if (existeAutorConMismoNombre) return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }
            context.Update(autor);
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x=>x.Id == id);
            if (!existe) return NotFound();
            context.Remove(new Autor() {Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
