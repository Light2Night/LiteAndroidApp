package com.it.liteapp.security;

public interface JwtSecurityService {
    void saveJwtToken(String token);

    String getToken();

    void deleteToken();

    boolean isAuth();
}
