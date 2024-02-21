using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Models
{
    /// <summary>
    /// Represents a user in the application,
    /// inheriting from IdentityUser provided by Microsoft.AspNet.Identity.
    /// </summary>
    public abstract class User : IdentityUser
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        [MaxLength(256)]
        public required string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        [MaxLength(256)]
        public required string LastName { get; set; }

        /// <summary>
        /// Area of residence of the user.
        /// </summary>
        [MaxLength(256)]
        public required string AreaOfResidence { get; set; }
        public string SecretKey { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Indicates whether the user is locked from using the platform.
        /// </summary>
        public bool IsLocked { get; set; }
        public virtual ICollection<ReviewModel> Reviews { get; set; }
    }
}
