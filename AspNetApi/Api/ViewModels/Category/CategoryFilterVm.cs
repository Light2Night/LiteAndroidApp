﻿using Api.ViewModels.Pagination;

namespace Api.ViewModels.Category;

public class CategoryFilterVm : PaginationVm {
	public string? Name { get; set; }
}
