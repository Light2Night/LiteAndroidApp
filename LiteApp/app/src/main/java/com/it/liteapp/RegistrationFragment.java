package com.it.liteapp;

import static android.app.Activity.RESULT_OK;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.drawable.BitmapDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.google.android.material.textfield.TextInputEditText;
import com.it.liteapp.application.HomeApplication;
import com.it.liteapp.dto.JwtTokenResponseDTO;
import com.it.liteapp.network.RetrofitClient;
import com.it.liteapp.security.JwtSecurityService;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Objects;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegistrationFragment extends Fragment {
    private TextInputEditText emailInput;
    private TextInputEditText usernameInput;
    private TextInputEditText passwordInput;
    private TextInputEditText repeatPasswordInput;
    private TextInputEditText firstnameInput;
    private TextInputEditText lastnameInput;
    private ImageView image;
    private TextView errorText;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_registration, container, false);

        emailInput = view.findViewById(R.id.registration_email_input);
        usernameInput = view.findViewById(R.id.registration_username_input);
        passwordInput = view.findViewById(R.id.registration_password_input);
        repeatPasswordInput = view.findViewById(R.id.registration_repeatPassword_input);
        firstnameInput = view.findViewById(R.id.registration_firstname_input);
        lastnameInput = view.findViewById(R.id.registration_lastname_input);
        image = view.findViewById(R.id.registration_image);
        Button imageButton = view.findViewById(R.id.registration_imageButton);
        imageButton.setOnClickListener(v -> openImagePicker());
        Button registrationButton = view.findViewById(R.id.registration_button);
        registrationButton.setOnClickListener(this::onClickRegistration);
        errorText = view.findViewById(R.id.registration_error);

        Glide.with(this)
                .load("https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/corporate-user-icon.png")
                .apply(new RequestOptions().override(400))
                .into(image);

        return view;
    }

    private void onClickRegistration(View v) {
        String email = Objects.requireNonNull(emailInput.getText()).toString();
        String username = Objects.requireNonNull(usernameInput.getText()).toString();
        String password = Objects.requireNonNull(passwordInput.getText()).toString();
        String repeatPassword = Objects.requireNonNull(repeatPasswordInput.getText()).toString();
        String firstname = Objects.requireNonNull(firstnameInput.getText()).toString();
        String lastname = Objects.requireNonNull(lastnameInput.getText()).toString();

        if (!password.equals(repeatPassword)) {
            showError("Пароль не співпадає");
            return;
        }
        if (password.length() < 8) {
            showError("Пароль має бути не менше 8 символів");
            return;
        }

        RequestBody emailPart = RequestBody.create(email, MediaType.parse("text/plain"));
        RequestBody usernamePart = RequestBody.create(username, MediaType.parse("text/plain"));
        RequestBody passwordPart = RequestBody.create(password, MediaType.parse("text/plain"));
        RequestBody firstnamePart = RequestBody.create(firstname, MediaType.parse("text/plain"));
        RequestBody lastnamePart = RequestBody.create(lastname, MediaType.parse("text/plain"));

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

                showError("Invalid data");
            }

            @Override
            public void onFailure(@NonNull Call<JwtTokenResponseDTO> call, @NonNull Throwable throwable) {
                showError("Request failed");
            }
        };

        RetrofitClient.getInstance()
                .getAccountsApi()
                .registration(
                        emailPart,
                        usernamePart,
                        passwordPart,
                        firstnamePart,
                        lastnamePart,
                        getImagePart()
                )
                .enqueue(callback);
    }

    private void showError(String errorMessage) {
        errorText.setText(errorMessage);
        errorText.setVisibility(View.VISIBLE);
    }

    private void signIn() {
        if (getActivity() instanceof AuthorizationActivity) {
            ((AuthorizationActivity) getActivity()).signIn();
        }
    }

    private MultipartBody.Part getImagePart() {
        image.setDrawingCacheEnabled(true);
        image.buildDrawingCache();

        Bitmap bitmap = ((BitmapDrawable) image.getDrawable()).getBitmap();
        File imageFile = bitmapToFile(bitmap);

        RequestBody requestFile = RequestBody.create(imageFile, MediaType.parse("image/*"));

        return MultipartBody.Part.createFormData("image", imageFile.getName(), requestFile);
    }

    private File bitmapToFile(Bitmap bitmap) {
        Context context = HomeApplication.getAppContext();

        File filesDir = context.getFilesDir();
        File imageFile = new File(filesDir, "profile_image.png");

        try {
            OutputStream os = new FileOutputStream(imageFile);
            bitmap.compress(Bitmap.CompressFormat.PNG, 100, os);
            os.flush();
            os.close();
        } catch (IOException e) {
            Log.e("App", "Error writing bitmap", e);
        }

        return imageFile;
    }

    private void openImagePicker() {
        Intent intent = new Intent(Intent.ACTION_PICK);
        intent.setType("image/*");
        pickImageLauncher.launch(intent);
    }

    private final ActivityResultLauncher<Intent> pickImageLauncher = registerForActivityResult(
            new ActivityResultContracts.StartActivityForResult(),
            result -> {
                if (result.getResultCode() == RESULT_OK && result.getData() != null) {
                    Uri imageUri = result.getData().getData();
                    if (imageUri != null) {
                        Glide.with(this)
                                .load(imageUri)
                                .into(image);
                    }
                }
            }
    );
}
