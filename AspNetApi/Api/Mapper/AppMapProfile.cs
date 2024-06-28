using Api.ViewModels.Category;
using Api.ViewModels.Ingredient;
using Api.ViewModels.Pizza;
using Api.ViewModels.PizzaImage;
using Api.ViewModels.PizzaSize;
using AutoMapper;
using Model.Entities;

namespace Api.Mapper;

public class AppMapProfile : Profile {
	public AppMapProfile() {
		Category();
		Ingredient();
		PizzaIngredient();
		Pizza();
		PizzaImage();
		PizzaSize();
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

	private void PizzaIngredient() {
		CreateMap<PizzaIngredient, IngredientVm>()
			.ForMember(pi => pi.Id, opt => opt.MapFrom(src => src.Ingredient.Id))
			.ForMember(pi => pi.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
			.ForMember(pi => pi.Image, opt => opt.MapFrom(src => src.Ingredient.Image));
	}

	private void Pizza() {
		CreateMap<Pizza, PizzaVm>()
			.ForMember(
				p => p.Images,
				opt => opt.MapFrom(src => src.Images.OrderBy(pi => pi.Priority))
			);
		CreateMap<CreatePizzaVm, Pizza>()
			.ForMember(p => p.Images, opt => opt.Ignore())
			.ForMember(
				p => p.Ingredients,
				opt => opt.MapFrom(
					(src, dest) => (src.IngredientIds ?? [])
						.Select(id => new PizzaIngredient { Pizza = dest, IngredientId = id })
						.ToArray()
				)
			);
	}

	private void PizzaImage() {
		CreateMap<PizzaImage, PizzaImageShortVm>();
	}

	private void PizzaSize() {
		CreateMap<PizzaSize, PizzaSizeShortVm>()
			.ForMember(
				ps => ps.SizeName,
				opt => opt.MapFrom(src => src.Size.Name)
			);
	}
}