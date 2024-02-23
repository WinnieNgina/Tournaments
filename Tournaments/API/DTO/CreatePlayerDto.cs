using ModelsLibrary.Models;

namespace API.DTO
{
    public class CreatePlayerDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AreaOfResidence { get; set; }
        public DateTime DateOfBirth { get; set; }
        public PlayerStatus Status { get; set; }
        public string Password { get; set; }
    }
}
