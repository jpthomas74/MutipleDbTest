namespace LearnNewAspNetWebAppWithAuth.Models
{
    public class Department 
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ScopeGroupId { get; set; }
        public int ParentDepartment { get; set; }
    }
}
