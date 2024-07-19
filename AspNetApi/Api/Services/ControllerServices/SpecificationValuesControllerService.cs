using Api.Configurations;
using Api.Controllers;
using Api.DataTransferObjects;
using Api.Exceptions;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Pagination;
using Api.ViewModels.SpecificationValue;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class SpecificationValuesControllerService(
	DataContext context,
	IMapper mapper,
	ICacheService cacheService,
	IPaginationService<SpecificationValueVm, SpecificationValueFilterVm> pagination,
	IOptions<CacheExpirySeconds> options
) : ISpecificationValuesControllerService {
	private readonly int _cacheExpirySeconds = options.Value.SpecificationValuesController;

	public async Task<IEnumerable<SpecificationValueVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationValuesController.GetAll)
		);

		var entities = await cacheService.TryGetCacheAsync<IEnumerable<SpecificationValueVm>>(action);
		if (entities is not null)
			return entities;

		entities = await context.SpecificationValues
			.ProjectTo<SpecificationValueVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();
		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entities;
	}

	public async Task<PageVm<SpecificationValueVm>> GetPageAsync(SpecificationValueFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationValuesController.GetPage)
		);

		var page = await cacheService.TryGetCacheAsync<PageVm<SpecificationValueVm>>(action, vm);
		if (page is not null)
			return page;

		page = await pagination.GetPageAsync(vm);
		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return page;
	}

	public async Task<SpecificationValueVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationValuesController.GetById)
		);

		try {
			return await cacheService.GetCacheAsync<SpecificationValueVm?>(action, id);
		}
		catch (KeyIsNotExistsException) {
			var entity = await context.SpecificationValues
				.ProjectTo<SpecificationValueVm>(mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == id);

			await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromSeconds(_cacheExpirySeconds));

			return entity;
		}
	}

	public async Task CreateAsync(CreateSpecificationValueVm vm) {
		var entity = mapper.Map<SpecificationValue>(vm);

		await context.SpecificationValues.AddAsync(entity);

		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
	}

	public async Task UpdateAsync(UpdateSpecificationValueVm vm) {
		var entity = await context.SpecificationValues
			.FirstAsync(x => x.Id == vm.Id);

		entity.Value = vm.Value;
		entity.SpecificationNameId = vm.SpecificationNameId;

		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.SpecificationValues
			.FirstOrDefaultAsync(x => x.Id == id);

		if (entity is null)
			return;

		context.SpecificationValues.Remove(entity);
		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	private static string ControllerName => nameof(SpecificationValuesController);
}
