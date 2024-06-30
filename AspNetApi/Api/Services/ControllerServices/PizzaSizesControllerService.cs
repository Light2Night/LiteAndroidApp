using Api.Controllers;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.PizzaSize;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class PizzaSizesControllerService(
	DataContext context,
	IMapper mapper,
	ICacheService cacheService
) : IPizzaSizesControllerService {

	public async Task CreateAsync(CreatePizzaSizeVm vm) {
		var entity = mapper.Map<PizzaSize>(vm);

		await context.PizzaSizes.AddAsync(entity);

		await context.SaveChangesAsync();

		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	public async Task UpdateAsync(UpdatePizzaSizeVm vm) {
		var entity = mapper.Map<PizzaSize>(vm);

		context.PizzaSizes.Update(entity);

		await context.SaveChangesAsync();

		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	public async Task DeleteIfExistsAsync(long pizzaId, long sizeId) {
		var entity = await context.PizzaSizes.FirstOrDefaultAsync(x => x.PizzaId == pizzaId && x.SizeId == sizeId);

		if (entity is null)
			return;

		context.PizzaSizes.Remove(entity);
		await context.SaveChangesAsync();

		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}
}
