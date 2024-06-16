package com.it.liteapp;

import android.os.Bundle;
import android.view.MenuItem;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

import com.google.android.material.bottomnavigation.BottomNavigationView;

public class MainActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        BottomNavigationView bottomNavigationView = findViewById(R.id.bottom_navigation);

        bottomNavigationView.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                Fragment selectedFragment = null;

                int id = item.getItemId();

                if (id == R.id.m_home) {
                    selectedFragment = new HomeFragment();
                } else if (id == R.id.m_login) {
                    selectedFragment = new LoginFragment();
                } else if (id == R.id.m_register) {
                    selectedFragment = new RegistrationFragment();
                }

                if (selectedFragment != null) {
                    SetFragment(selectedFragment);
                }

                return true;
            }
        });

        if (savedInstanceState == null) {
            SetFragment(new HomeFragment());
        }
    }

    private void SetFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, fragment).commit();
    }
}