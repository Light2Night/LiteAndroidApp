package com.it.liteapp.network;

import com.it.liteapp.config.Config;
import com.it.liteapp.interceptors.JWTInterceptor;

import java.util.concurrent.TimeUnit;

import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class RetrofitClient {
    private static RetrofitClient instance;
    private Retrofit retrofit;

    public RetrofitClient() {
        HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
        interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);

        OkHttpClient.Builder client = new OkHttpClient.Builder()
                .connectTimeout(20, TimeUnit.SECONDS)
                .writeTimeout(20, TimeUnit.SECONDS)
                .readTimeout(20, TimeUnit.SECONDS)
                .addInterceptor(new JWTInterceptor())
                .addInterceptor(interceptor);

        retrofit = new Retrofit.Builder()
                .baseUrl(Config.BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .client(client.build())
                .build();
    }

    public static RetrofitClient getInstance() {
        if (instance == null) {
            instance = new RetrofitClient();
        }
        return instance;
    }

    public CategoriesApi getCategoriesApi() {
        return retrofit.create(CategoriesApi.class);
    }

    public PizzasApi getPizzasApi() {
        return retrofit.create(PizzasApi.class);
    }

    public AccountsApi getAccountsApi() {
        return retrofit.create(AccountsApi.class);
    }
}
