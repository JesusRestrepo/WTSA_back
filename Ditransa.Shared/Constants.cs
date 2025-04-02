namespace Ditransa.Shared
{
    public class Constants
    {
        public static int PAGE_SIZE = 10;

        public static int MAX_PAGE_SIZE = int.MaxValue;
    }

    public class RelationConstants
    {
        public static string Driver = "Conductor";

        public static string Tenedor = "Tenedor";

        public static string PersonalReference = "Referencia Personal";

        public static string FamilyReference = "Referencia Familiar";
    }

    public class RoleConstants
    {
        public static string Administrator = "Administrador";

        public static string BasicUser = "Usuario Basico";

        public static string Transporter = "Transportador";
    }

    public class CustomerState
    {
        public static string NewCustomerRequest = "Pendiente";
    }
}