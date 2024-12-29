using RHCore.Data.Enum;

namespace RHCore.Helpers
{
    public class VacationStatusHelper
    {
        public static string Translate(VacationStatus status)
        {
            return status switch
            {
                VacationStatus.Pending => "Pendente",
                VacationStatus.Ongoing => "Em Andamento",
                VacationStatus.Completed => "Concluída",
                _ => "Desconhecido"
            };
        }
    }
}
