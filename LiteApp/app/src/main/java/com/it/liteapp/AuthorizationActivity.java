package com.it.liteapp;

import android.content.Intent;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.it.liteapp.application.HomeApplication;

public class AuthorizationActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authorization);

        BottomNavigationView bottomNavigationView = findViewById(R.id.authorization_bottom_navigation);

        bottomNavigationView.setOnItemSelectedListener(item -> {
            Fragment selectedFragment = GetFragmentById(item.getItemId());

            if (selectedFragment != null) {
                SetFragment(selectedFragment);
            }

            return true;
        });

        if (savedInstanceState == null) {
            SetFragment(new LoginFragment());
        }
    }

    private void SetFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction().replace(R.id.authorization_fragment_container, fragment).commit();
    }

    private Fragment GetFragmentById(int id) {
        if (id == R.id.m_login) {
            return new LoginFragment();
        } else if (id == R.id.m_register) {
            return new RegistrationFragment();
        }

        return null;
    }

    public void signIn() {
        Intent intent = new Intent(this, MainActivity.class);

        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);

        startActivity(intent);
        finish();
    }
}