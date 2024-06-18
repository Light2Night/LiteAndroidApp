package com.it.liteapp.network;


import com.it.liteapp.dto.CategoryItemDTO;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;

public interface CategoriesApi {
    @GET("/api/Categories/GetAll")
    public Call<List<CategoryItemDTO>> list();
}
