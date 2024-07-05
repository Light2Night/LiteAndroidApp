package com.it.liteapp.dto;

public class JwtTokenResponseDTO {
    public String token;

    public JwtTokenResponseDTO() {
    }

    public JwtTokenResponseDTO(String token) {
        this.token = token;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}
