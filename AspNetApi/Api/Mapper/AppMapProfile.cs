using Api.ViewModels.Category;
using AutoMapper;
using Model.Entities;

namespace Api.Mapper;

public class AppMapProfile : Profile {
	public AppMapProfile() {
		CreateMap<Category, CategoryVm>();
		CreateMap<CreateCategoryVm, Category>()
			.ForMember(c => c.Image, opt => opt.Ignore());
	}
}