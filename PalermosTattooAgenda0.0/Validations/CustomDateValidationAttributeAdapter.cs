using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using PalermoAgenda.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace PalermosTattooAgenda0.Web.Validations
{
    // Agrega al <input> los atributos data-val-fechaminima para que jQuery valide la fecha en el navegador
    public class CustomDateValidationAttributeAdapter : AttributeAdapterBase<CustomDateValidationAttribute>
    {
        public CustomDateValidationAttributeAdapter(CustomDateValidationAttribute attribute, IStringLocalizer? stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-fechaminima", GetErrorMessage(context));
            // Se envía la fecha de hoy del servidor para no depender de la hora del equipo del usuario
            MergeAttribute(context.Attributes, "data-val-fechaminima-hoy", DateTime.Today.ToString("yyyy-MM-dd"));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
            => GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
    }

    // Registra el adaptador anterior; los demás atributos siguen usando los adaptadores de ASP.NET Core
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly ValidationAttributeAdapterProvider _base = new();

        public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
        {
            if (attribute is CustomDateValidationAttribute fecha)
                return new CustomDateValidationAttributeAdapter(fecha, stringLocalizer);
            return _base.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
