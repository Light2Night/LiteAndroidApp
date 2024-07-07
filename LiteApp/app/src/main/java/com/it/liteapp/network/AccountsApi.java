package com.it.liteapp.network;

import com.it.liteapp.dto.JwtTokenResponseDTO;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;

public interface AccountsApi {
    @FormUrlEncoded
    @POST("/api/Accounts/SignIn")
    Call<JwtTokenResponseDTO> signIn(
            @Field("email") String email,
            @Field("password") String password
    );

    @Multipart
    @POST("/api/Accounts/Registration")
    Call<JwtTokenResponseDTO> registration(
            @Part("email") RequestBody email,
            @Part("username") RequestBody username,
            @Part("password") RequestBody password,
            @Part("firstname") RequestBody firstname,
            @Part("lastname") RequestBody lastname,
            @Part MultipartBody.Part image
    );
}
