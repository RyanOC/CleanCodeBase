using System;

namespace Core.Entities
{
    public class AccessToken
    {
        public string token_type { get; set; }
        public string subject_type { get; set; }
        public string subject { get; set; }
        public string subject_name { get; set; }
        public string alternate_subject { get; set; }
        public string alternate_subject_name { get; set; }
        public string token { get; set; }
        public DateTime token_expires { get; set; }
    }
}