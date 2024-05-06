namespace AMNSystemsERP.CL.Models.Commons
{
    public class Base64Request
    {
        public string FilePath { get; set; }
    }
    public class Base64Response
    {
        public string FilePath { get; set; }
        public string Base64String { get; set; }
    }
}