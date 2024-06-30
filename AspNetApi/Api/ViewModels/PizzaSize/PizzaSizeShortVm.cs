using Api.ViewModels.Size;

namespace Api.ViewModels.PizzaSize;

public class PizzaSizeShortVm {
	public SizeVm Size { get; set; } = null!;

	public decimal Price { get; set; }
}
