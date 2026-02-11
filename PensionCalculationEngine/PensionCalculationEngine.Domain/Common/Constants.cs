namespace PensionCalculationEngine.Domain.Common;

public static class Constants
{
    public static class MutationDefinitions
    {
        public const string CreateDossier = "create_dossier";
        public const string AddPolicy = "add_policy";
        public const string ApplyIndexation = "apply_indexation";
    }

    public static class MutationPropertiesDefinitions
    {
        public static class CreateDossierDefinitions
        {
            public const string DossierId = "dossier_id";
            public const string PersonId = "person_id";
            public const string Name = "name";
            public const string BirthDate = "birth_date";
        }
    }

}