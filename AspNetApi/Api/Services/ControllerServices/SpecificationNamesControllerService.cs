using Api.Configurations;
using Api.Controllers;
using Api.DataTransferObjects;
using Api.Exceptions;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Pagination;
using Api.ViewModels.SpecificationName;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class SpecificationNamesControllerService(
	DataContext context,
	IMapper mapper,
	ICacheService cacheService,
	IPaginationService<SpecificationNameVm, SpecificationNameFilterVm> pagination,
	IOptions<CacheExpirySeconds> options
) : ISpecificationNamesControllerService {
	private readonly int _cacheExpirySeconds = options.Value.SpecificationNamesController;

	public async Task<IEnumerable<SpecificationNameVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationNamesController.GetAll)
		);

		var entities = await cacheService.TryGetCacheAsync<IEnumerable<SpecificationNameVm>>(action);
		if (entities is not null)
			return entities;

		entities = await context.SpecificationNames
			.ProjectTo<SpecificationNameVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();
		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entities;
	}

	public async Task<PageVm<SpecificationNameVm>> GetPageAsync(SpecificationNameFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationNamesController.GetPage)
		);

		var page = await cacheService.TryGetCacheAsync<PageVm<SpecificationNameVm>>(action, vm);
		if (page is not null)
			return page;

		page = await pagination.GetPageAsync(vm);
		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return page;
	}

	public async Task<SpecificationNameVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(SpecificationNamesController.GetById)
		);

		try {
			return await cacheService.GetCacheAsync<SpecificationNameVm?>(action, id);
		}
		catch (KeyIsNotExistsException) {
			var entity = await context.SpecificationNames
				.ProjectTo<SpecificationNameVm>(mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == id);

			await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromSeconds(_cacheExpirySeconds));

			return entity;
		}
	}

	public async Task CreateAsync(CreateSpecificationNameVm vm) {
		var entity = mapper.Map<SpecificationName>(vm);

		await context.SpecificationNames.AddAsync(entity);

		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
	}

	public async Task UpdateAsync(UpdateSpecificationNameVm vm) {
		var entity = await context.SpecificationNames
			.FirstAsync(x => x.Id == vm.Id);

		entity.Name = vm.Name;
		entity.CategoryId = vm.CategoryId;

		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.SpecificationNames
			.FirstOrDefaultAsync(x => x.Id == id);

		if (entity is null)
			return;

		context.SpecificationNames.Remove(entity);
		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
		await cacheService.DeleteCacheByControllerAsync(nameof(SpecificationValuesController));
		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	private static string ControllerName => nameof(SpecificationNamesController);
}
