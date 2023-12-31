namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        public void HacerTarea();
    }
    public class ServicioA:IServicio       
    {
        private readonly ILogger<ServicioA> logger;

        public ServicioA(ILogger <ServicioA> logger)
        {
            this.logger = logger;
        }
        public void HacerTarea()
        {
        }
    }
    public class ServicioB : IServicio
    {
        public void HacerTarea()
        {
        }
    }
}
