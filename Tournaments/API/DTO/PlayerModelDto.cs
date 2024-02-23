using ModelsLibrary.Models;

namespace API.DTO
{
    public class PlayerModelDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AreaOfResidence { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NextOfKinCOntactInfo { get; set; }
        public PlayerStatus Status { get; set; }
        public IEnumerable<string> TeamNames { get; set; }
    }
}
