// Validaciones del lado del cliente (se carga después de jquery.validate y jquery.validate.unobtrusive)
(function ($) {
    if (!$ || !$.validator) return;

    // Mensajes predeterminados de jQuery Validation en español
    // (solo se usan cuando el campo no trae su propio mensaje desde el servidor)
    $.extend($.validator.messages, {
        required: "Este campo es requerido.",
        remote: "Por favor, corrija este campo.",
        email: "Ingrese un correo electrónico válido.",
        url: "Ingrese una dirección web válida.",
        date: "Ingrese una fecha válida.",
        dateISO: "Ingrese una fecha válida (AAAA-MM-DD).",
        number: "Ingrese un número válido.",
        digits: "Ingrese solo dígitos.",
        equalTo: "Ingrese el mismo valor de nuevo.",
        maxlength: $.validator.format("No puede tener más de {0} caracteres."),
        minlength: $.validator.format("Debe tener al menos {0} caracteres."),
        rangelength: $.validator.format("Debe tener entre {0} y {1} caracteres."),
        range: $.validator.format("Ingrese un valor entre {0} y {1}."),
        max: $.validator.format("Ingrese un valor menor o igual a {0}."),
        min: $.validator.format("Ingrese un valor mayor o igual a {0}."),
        step: $.validator.format("Ingrese un múltiplo de {0}.")
    });

    // [CustomDateValidation]: la fecha no puede ser anterior a hoy
    // "hoy" viene del servidor en formato yyyy-MM-dd (data-val-fechaminima-hoy)
    $.validator.addMethod("fechaminima", function (value, element, hoy) {
        if (this.optional(element)) return true;
        // Las fechas yyyy-MM-dd se pueden comparar como texto
        return value >= hoy;
    });

    $.validator.unobtrusive.adapters.add("fechaminima", ["hoy"], function (options) {
        options.rules["fechaminima"] = options.params.hoy;
        options.messages["fechaminima"] = options.message;
    });
})(window.jQuery);
