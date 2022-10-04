namespace Tp1WebApp.Models
{
    public class Libro
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_publicacion { get; set; }
        public string portada { get; set; }
        public int autorId { get; set; }
        public int generoId { get; set; }
        public Genero? genero { get; set; }
        public Autor? autor { get; set; }

    }
}
