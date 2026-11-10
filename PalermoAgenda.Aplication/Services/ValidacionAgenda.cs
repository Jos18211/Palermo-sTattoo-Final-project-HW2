namespace PalermoAgenda.Application.Services
{
    /// <summary>
    /// Reglas compartidas por Horario y Bloqueo.
    /// </summary>
    public static class ValidacionAgenda
    {
        /// <summary>
        /// Dos rangos [inicioA, finA) y [inicioB, finB) se sobreponen si cada uno
        /// empieza antes de que el otro termine. Se usa &lt; estricto: rangos que
        /// solo se tocan en un extremo (08:00-12:00 y 12:00-13:00) NO se sobreponen.
        /// </summary>
        public static bool SeSobreponen(TimeOnly inicioA, TimeOnly finA, TimeOnly inicioB, TimeOnly finB)
            => inicioA < finB && inicioB < finA;

        /// <summary>
        /// Rango efectivo de un bloqueo: si es todo el día cubre las 24 horas.
        /// </summary>
        public static (TimeOnly Inicio, TimeOnly Fin) RangoBloqueo(bool todoElDia, TimeOnly? inicio, TimeOnly? fin)
            => todoElDia ? (TimeOnly.MinValue, TimeOnly.MaxValue) : (inicio!.Value, fin!.Value);

        /// <summary>
        /// Valida fecha vigente y rango de horas. Devuelve el mensaje de error o null.
        /// </summary>
        public static string? ValidarFechaYRango(DateOnly fecha, TimeOnly inicio, TimeOnly fin)
        {
            // Se compara contra la fecha de hoy SIN hora (DateTime.Today), para que hoy sea válido
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            if (fecha < hoy)
                return "La fecha no puede ser anterior a la fecha actual";
            if (fin <= inicio)
                return "La hora de fin debe ser mayor que la hora de inicio";
            if (fecha == hoy && inicio <= TimeOnly.FromDateTime(DateTime.Now))
                return "Para la fecha de hoy, la hora de inicio debe ser posterior a la hora actual";
            return null;
        }

        /// <summary>
        /// 1 = lunes ... 7 = domingo (en .NET el domingo es 0).
        /// </summary>
        public static byte DiaSemana(DateOnly fecha)
        {
            int dia = (int)fecha.DayOfWeek;
            return (byte)(dia == 0 ? 7 : dia);
        }

        public static string Rango(TimeOnly inicio, TimeOnly fin) => $"{inicio:HH\\:mm} a {fin:HH\\:mm}";
    }
}
