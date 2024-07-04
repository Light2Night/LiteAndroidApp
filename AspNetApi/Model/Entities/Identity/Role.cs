using Microsoft.AspNetCore.Identity;

namespace Model.Entities.Identity;

public class Role : IdentityRole<long> {
	public ICollection<UserRole> Users { get; set; } = null!;
}
