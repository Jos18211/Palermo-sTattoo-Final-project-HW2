using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.ComponentModel.DataAnnotations;

namespace PalermosTattooAgenda0.Web.Validations
{
    /// <summary>
    /// Traduce al español los mensajes de validación que ASP.NET Core muestra en inglés:
    /// atributos sin ErrorMessage (por ejemplo [StringLength(50)]), el "requerido" implícito
    /// de los tipos no anulables (int, decimal, DateOnly...) y los errores de conversión de datos.
    /// Los atributos que ya tienen ErrorMessage conservan su mensaje.
    /// </summary>
    public class MensajesValidacionEspanolProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            var validadores = context.ValidationMetadata.ValidatorMetadata;

            foreach (var atributo in validadores.OfType<ValidationAttribute>())
            {
                if (string.IsNullOrEmpty(atributo.ErrorMessage) && atributo.ErrorMessageResourceType == null)
                    atributo.ErrorMessage = Mensaje(atributo);
            }

            // Requerido implícito de los tipos de valor no anulables (lo agrega ASP.NET Core en inglés)
            var tipo = context.Key.ModelType;
            if (context.Key.MetadataKind == ModelMetadataKind.Property
                && tipo.IsValueType && Nullable.GetUnderlyingType(tipo) == null
                && !validadores.OfType<RequiredAttribute>().Any())
            {
                validadores.Add(new RequiredAttribute { ErrorMessage = "El campo {0} es requerido" });
            }
        }

        private static string Mensaje(ValidationAttribute atributo) => atributo switch
        {
            RequiredAttribute => "El campo {0} es requerido",
            StringLengthAttribute s when s.MinimumLength > 0 => "El campo {0} debe tener entre {2} y {1} caracteres",
            StringLengthAttribute => "El campo {0} no puede tener más de {1} caracteres",
            RangeAttribute => "El campo {0} debe estar entre {1} y {2}",
            MaxLengthAttribute => "El campo {0} no puede tener más de {1} caracteres",
            MinLengthAttribute => "El campo {0} debe tener al menos {1} caracteres",
            EmailAddressAttribute => "El campo {0} no es un correo electrónico válido",
            PhoneAttribute => "El campo {0} no es un número de teléfono válido",
            UrlAttribute => "El campo {0} no es una dirección web válida",
            RegularExpressionAttribute => "El campo {0} no tiene un formato válido",
            CompareAttribute => "El campo {0} no coincide con {1}",
            _ => "El campo {0} no es válido"
        };

        // Errores de conversión del model binding (por ejemplo, letras en un campo numérico)
        public static void Configurar(MvcOptions options)
        {
            var m = options.ModelBindingMessageProvider;
            m.SetValueMustBeANumberAccessor(campo => $"El campo {campo} debe ser un número");
            m.SetNonPropertyValueMustBeANumberAccessor(() => "El campo debe ser un número");
            // Campo numérico o de fecha que se envía vacío
            m.SetValueMustNotBeNullAccessor(valor => "Este campo es requerido");
            m.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"El valor '{valor}' no es válido para {campo}");
            m.SetNonPropertyAttemptedValueIsInvalidAccessor(valor => $"El valor '{valor}' no es válido");
            m.SetUnknownValueIsInvalidAccessor(campo => $"El valor indicado no es válido para {campo}");
            m.SetNonPropertyUnknownValueIsInvalidAccessor(() => "El valor indicado no es válido");
            m.SetValueIsInvalidAccessor(valor => $"El valor '{valor}' no es válido");
            m.SetMissingBindRequiredValueAccessor(campo => $"Falta un valor para {campo}");
            m.SetMissingKeyOrValueAccessor(() => "Falta un valor requerido");
            m.SetMissingRequestBodyRequiredValueAccessor(() => "La solicitud no tiene datos");

            options.ModelMetadataDetailsProviders.Add(new MensajesValidacionEspanolProvider());
        }
    }
}
