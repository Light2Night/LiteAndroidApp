package com.it.liteapp;

import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

import com.google.android.material.bottomnavigation.BottomNavigationView;

public class MainActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        BottomNavigationView bottomNavigationView = findViewById(R.id.bottom_navigation);

        bottomNavigationView.setOnItemSelectedListener(item -> {
            Fragment selectedFragment = GetFragmentById(item.getItemId());

            if (selectedFragment != null) {
                SetFragment(selectedFragment);
            }

            return true;
        });

        if (savedInstanceState == null) {
            SetFragment(new HomeFragment());
        }
    }

    private void SetFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, fragment).commit();
    }

    private Fragment GetFragmentById(int id) {
        if (id == R.id.m_home) {
            return new HomeFragment();
        } else if (id == R.id.m_account) {
            return new AccountFragment();
        }

        return null;
    }
}