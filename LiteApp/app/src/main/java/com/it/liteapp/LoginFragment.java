package com.it.liteapp;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import com.google.android.material.textfield.TextInputEditText;
import com.it.liteapp.application.HomeApplication;
import com.it.liteapp.dto.JwtTokenResponseDTO;
import com.it.liteapp.network.RetrofitClient;
import com.it.liteapp.security.JwtSecurityService;

import java.util.Objects;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginFragment extends Fragment {
    private TextInputEditText emailInput;
    private TextInputEditText passwordInput;
    private TextView errorText;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_login, container, false);

        Button button = view.findViewById(R.id.login_button);
        button.setOnClickListener(this::onClickSignIn);
        emailInput = view.findViewById(R.id.login_email_input);
        passwordInput = view.findViewById(R.id.login_password_input);
        errorText = view.findViewById(R.id.login_error);

        return view;
    }

    public void onClickSignIn(View v) {
        String email = Objects.requireNonNull(emailInput.getText()).toString();
        String password = Objects.requireNonNull(passwordInput.getText()).toString();

        Callback<JwtTokenResponseDTO> callback = new Callback<JwtTokenResponseDTO>() {
            @Override
            public void onResponse(@NonNull Call<JwtTokenResponseDTO> call, @NonNull Response<JwtTokenResponseDTO> response) {
                if (response.isSuccessful()) {
                    JwtTokenResponseDTO result = response.body();
                    JwtSecurityService jwt = HomeApplication.getInstance();
                    assert result != null;
                    jwt.saveJwtToken(result.getToken());

                    signIn();
                    return;
                }

                int code = response.code();

                if (code == 401)
                    showError("Incorrect email or password");
                else
                    showError("Invalid data");
            }

            @Override
            public void onFailure(@NonNull Call<JwtTokenResponseDTO> call, @NonNull Throwable throwable) {
                showError("Request failed");
            }
        };

        RetrofitClient.getInstance()
                .getAccountsApi()
                .signIn(email, password)
                .enqueue(callback);
    }

    private void showError(String errorMessage) {
        errorText.setText(errorMessage);
        errorText.setVisibility(View.VISIBLE);
    }

    public void signIn() {
        if (getActivity() instanceof AuthorizationActivity) {
            ((AuthorizationActivity) getActivity()).signIn();
        }
    }
}