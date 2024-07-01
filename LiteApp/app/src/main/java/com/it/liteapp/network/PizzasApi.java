package com.it.liteapp.network;

import com.it.liteapp.dto.PizzasPageDTO;

import retrofit2.Call;
import retrofit2.http.GET;

public interface PizzasApi {
    @GET("/api/Pizzas/GetPage")
    public Call<PizzasPageDTO> page();
}
