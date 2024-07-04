using Model.Entities.Identity;

namespace Api.Services.Interfaces;

public interface IJwtTokenService {
	Task<string> CreateTokenAsync(User user);
}
