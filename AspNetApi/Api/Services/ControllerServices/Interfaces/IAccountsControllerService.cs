using Api.ViewModels.Account;
using Model.Entities.Identity;

namespace Api.Services.ControllerServices.Interfaces;

public interface IAccountsControllerService {
	Task<User> SignUpAsync(RegisterVm vm);
	Task<User> GoogleSignInAsync(GoogleSignInVm model);
}
