package com.it.liteapp;

import android.content.Intent;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;

import com.it.liteapp.application.HomeApplication;

public class LauncherActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        startActivity(getStartIntent());
        finish();
    }

    private Intent getStartIntent() {
        HomeApplication application = HomeApplication.getInstance();

        if (application.isAuth()) {
            return new Intent(this, MainActivity.class);
        } else {
            return new Intent(this, LoginFragment.class);
        }
    }
}