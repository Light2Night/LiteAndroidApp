package com.it.liteapp.interceptors;

import androidx.annotation.NonNull;

import com.it.liteapp.application.HomeApplication;
import com.it.liteapp.security.JwtSecurityService;

import java.io.IOException;

import okhttp3.Interceptor;
import okhttp3.Request;
import okhttp3.Response;

public class JWTInterceptor implements Interceptor {
    @NonNull
    @Override
    public Response intercept(Chain chain) throws IOException {
        JwtSecurityService jwtService = HomeApplication.getInstance();

        String token = jwtService.getToken();

        Request.Builder builder = chain.request().newBuilder();

        if (token != null && !token.isEmpty()) {
            builder.header("Authorization", "Bearer " + token);
        }

        return chain.proceed(builder.build());
    }
}
