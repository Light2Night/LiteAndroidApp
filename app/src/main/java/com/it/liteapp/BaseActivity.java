package com.it.liteapp;

import android.content.Intent;
import android.view.Menu;
import android.view.MenuItem;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;

public class BaseActivity extends AppCompatActivity {
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int select = item.getItemId();

        if (select == R.id.m_main) {
            Intent intent = new Intent(BaseActivity.this, MainActivity.class);
            startActivity(intent);
            return true;
        } else if (select == R.id.m_register) {
            Intent intent = new Intent(BaseActivity.this, RegistrationActivity.class);
            startActivity(intent);
            return true;
        } else if (select == R.id.m_login) {
            Intent intent = new Intent(BaseActivity.this, LoginActivity.class);
            startActivity(intent);
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }
}