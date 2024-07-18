using Api.ViewModels.Account;
using Api.ViewModels.Category;
using Api.ViewModels.Ingredient;
using Api.ViewModels.Pizza;
using Api.ViewModels.PizzaImage;
using Api.ViewModels.PizzaSize;
using Api.ViewModels.Size;
using Api.ViewModels.SpecificationValue;
using AutoMapper;
using Model.Entities;
using Model.Entities.Identity;

namespace Api.Mapper;

public class AppMapProfile : Profile {
	public AppMapProfile() {
		Account();
		Category();
		Ingredient();
		PizzaIngredient();
		Pizza();
		PizzaImage();
		PizzaSize();
		Size();
		SpecificationValue();
	}

	private void Account() {
		CreateMap<RegisterVm, User>();
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
			)
			.ForMember(
				p => p.SpecificationValues,
				opt => opt.MapFrom(
					(src, dest) => (src.SpecificationValueIds ?? [])
						.Select(id => new PizzaSpecificationValue { Pizza = dest, SpecificationValueId = id })
						.ToArray()
				)
			);
	}

	private void PizzaImage() {
		CreateMap<PizzaImage, PizzaImageShortVm>();
	}

	private void PizzaSize() {
		CreateMap<PizzaSize, PizzaSizeShortVm>();
		CreateMap<CreatePizzaSizeVm, PizzaSize>();
		CreateMap<UpdatePizzaSizeVm, PizzaSize>();
	}

	private void Size() {
		CreateMap<Size, SizeVm>();
		CreateMap<CreateSizeVm, Size>();
	}

	private void SpecificationValue() {
		CreateMap<SpecificationValue, SpecificationValueVm>();
		CreateMap<PizzaSpecificationValue, SpecificationValueVm>()
			.ForMember(sv => sv.Id, opt => opt.MapFrom(src => src.SpecificationValueId))
			.ForMember(sv => sv.Value, opt => opt.MapFrom(src => src.SpecificationValue.Value))
			.ForMember(sv => sv.SpecificationNameId, opt => opt.MapFrom(src => src.SpecificationValue.SpecificationNameId)
		);
	}
}