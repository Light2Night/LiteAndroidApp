package com.it.liteapp.network;

import com.it.liteapp.dto.JwtTokenResponseDTO;
import com.it.liteapp.dto.PizzasPageDTO;

import retrofit2.Call;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface AccountsApi {
    @FormUrlEncoded
    @POST("/api/Accounts/SignIn")
    Call<JwtTokenResponseDTO> signIn(
            @Field("email") String email,
            @Field("password") String password
    );
}
