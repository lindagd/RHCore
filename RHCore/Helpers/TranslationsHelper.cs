using RHCore.Data.Enum;

namespace RHCore.Helpers
{
    public class TranslationsHelper
    {
        public static string TranslateAttr(string? fieldName)
        {
            return fieldName switch
            {
                "Name" => "Nome",
                "Role" => "Cargo",
                "AdmissionDate" => "Data de Admissão",
                "Paycheck" => "Salário",
                "IsActive" => "Status",
                _ => "Desconhecido"
            };
        }
    }
}
