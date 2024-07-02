package com.it.liteapp.network;

import com.it.liteapp.dto.PizzaItemDTO;
import com.it.liteapp.dto.PizzasPageDTO;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface PizzasApi {
    @GET("/api/Pizzas/GetPage")
    Call<PizzasPageDTO> page(
            @Query("categoryId") long categoryId
    );

    @GET("/api/Pizzas/GetById/{id}")
    Call<PizzaItemDTO> pizza(
            @Path("id") long id
    );
}
