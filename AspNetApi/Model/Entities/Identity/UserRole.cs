using Microsoft.AspNetCore.Identity;

namespace Model.Entities.Identity;

public class UserRole : IdentityUserRole<long> {
	public User User { get; set; } = null!;

	public Role Role { get; set; } = null!;
}
