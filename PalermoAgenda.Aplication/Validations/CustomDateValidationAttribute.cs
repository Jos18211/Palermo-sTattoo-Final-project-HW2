using System;
using System.ComponentModel.DataAnnotations;

namespace PalermoAgenda.Application.Validations
{
    /// <summary>
    /// Valida que la fecha no sea anterior a la fecha actual (la fecha de hoy sí es válida).
    /// Funciona con DateOnly y DateTime. En el navegador la valida el adaptador
    /// CustomDateValidationAttributeAdapter (proyecto Web) junto con wwwroot/js/validaciones.js.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        public CustomDateValidationAttribute()
            : base("El campo {0} debe ser una fecha mayor a la actual.")
        {
        }

        public override bool IsValid(object? value)
        {
            // Si viene vacío lo valida [Required]
            if (value == null) return true;

            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return value switch
            {
                DateOnly fecha => fecha >= hoy,
                DateTime fecha => DateOnly.FromDateTime(fecha) >= hoy,
                _ => false
            };
        }
    }
}
