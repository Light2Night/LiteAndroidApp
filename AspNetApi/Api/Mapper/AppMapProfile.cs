using Api.ViewModels.Category;
using Api.ViewModels.Ingredient;
using AutoMapper;
using Model.Entities;

namespace Api.Mapper;

public class AppMapProfile : Profile {
	public AppMapProfile() {
		Category();

		Ingredient();
	}

	private void Category() {
		CreateMap<Category, CategoryVm>();
		CreateMap<CreateCategoryVm, Category>()
			.ForMember(c => c.Image, opt => opt.Ignore());
	}

	private void Ingredient() {
		CreateMap<Ingredient, IngredientVm>();
		CreateMap<CreateIngredientVm, Ingredient>()
			.ForMember(i => i.Image, opt => opt.Ignore());
	}
}