using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLazaro.Models
{
    public partial class Items
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = null!;



        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = null!;
    }


}
