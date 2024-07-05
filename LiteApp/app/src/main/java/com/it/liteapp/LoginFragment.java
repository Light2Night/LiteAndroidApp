package com.it.liteapp;

import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.google.android.material.textfield.TextInputEditText;
import com.it.liteapp.application.HomeApplication;
import com.it.liteapp.dto.JwtTokenResponseDTO;
import com.it.liteapp.network.RetrofitClient;
import com.it.liteapp.security.JwtSecurityService;

import java.io.IOException;
import java.util.Objects;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginFragment extends Fragment implements View.OnClickListener {
    Button button;
    TextInputEditText emailInput;
    TextInputEditText passwordInput;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_login, container, false);

        button = view.findViewById(R.id.login_button);
        button.setOnClickListener(this);
        emailInput = view.findViewById(R.id.login_email_input);
        passwordInput = view.findViewById(R.id.login_password_input);

        return view;
    }

    @Override
    public void onClick(View v) {
        String email = Objects.requireNonNull(emailInput.getText()).toString();
        String password = Objects.requireNonNull(passwordInput.getText()).toString();

        RetrofitClient.getInstance()
                .getAccountsApi()
                .signIn(email, password)
                .enqueue(new Callback<JwtTokenResponseDTO>() {
                    @Override
                    public void onResponse(Call<JwtTokenResponseDTO> call, Response<JwtTokenResponseDTO> response) {
                        if (response.isSuccessful()) {
                            JwtTokenResponseDTO result = response.body();
                            JwtSecurityService jwt = HomeApplication.getInstance();
                            assert result != null;
                            jwt.saveJwtToken(result.getToken());
                            Log.d("API onResponse", "Success: " + result.getToken());
                        } else {
                            try {
                                String errorBody = response.errorBody().string();
                                Log.e("API Error", "Error: " + errorBody);
                            } catch (IOException e) {
                                e.printStackTrace();
                            }
                        }
                    }

                    @Override
                    public void onFailure(Call<JwtTokenResponseDTO> call, Throwable throwable) {
                        Log.e("API onFailure", "Error: " + throwable.getMessage());
                    }
                });
    }
}