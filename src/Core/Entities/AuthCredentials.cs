namespace Core.Entities
{
    public class AuthCredentials
    {
        public string client_id { get; set; }
        public string tenant_id { get; set; }
        public string branch_id { get; set; }
        public string subject_type { get; set; }
        public string subject { get; set; }
        public string password { get; set; }
        public string alternate_subject { get; set; }
    }
}