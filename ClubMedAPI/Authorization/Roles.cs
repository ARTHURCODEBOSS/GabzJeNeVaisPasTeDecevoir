namespace ClubMedAPI.Authorization
{
    public static class Roles
    {
        public const string Client = "CLIENT";
        public const string Vente = "VENTE";
        public const string DirecteurVente = "DIRECTEUR-VENTE";
        public const string Marketing = "MARKETING";
        public const string DirecteurMarketing = "DIRECTEUR-MARKETING";

        // Combinaisons utiles pour les policies
        public const string AllStaff = "VENTE,DIRECTEUR-VENTE,MARKETING,DIRECTEUR-MARKETING";
        public const string VenteTeam = "VENTE,DIRECTEUR-VENTE";
        public const string MarketingTeam = "MARKETING,DIRECTEUR-MARKETING";
        public const string Directors = "DIRECTEUR-VENTE,DIRECTEUR-MARKETING";
        public const string All = "CLIENT,VENTE,DIRECTEUR-VENTE,MARKETING,DIRECTEUR-MARKETING";
    }
}