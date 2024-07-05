package com.it.liteapp.application;

import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;

import com.it.liteapp.security.JwtSecurityService;

public class HomeApplication extends Application implements JwtSecurityService {
    private static HomeApplication instance;
    private static Context appContext;
    private static final String PREFERENCE_NAME = "jwtStore";
    private static final String KEY_NAME = "token";

    @Override
    public void onCreate() {
        super.onCreate();
        instance = this;
        appContext = getApplicationContext();
    }

    public static HomeApplication getInstance() {
        return instance;
    }

    public static Context getAppContext() {
        return appContext;
    }

    @Override
    public void saveJwtToken(String token) {
        SharedPreferences prefs;
        SharedPreferences.Editor edit;

        prefs = instance.getSharedPreferences(PREFERENCE_NAME, MODE_PRIVATE);
        edit = prefs.edit();

        edit.putString(KEY_NAME, token);
        edit.apply();
    }

    @Override
    public String getToken() {
        SharedPreferences prefs = instance.getSharedPreferences(PREFERENCE_NAME, Context.MODE_PRIVATE);

        String token = prefs.getString(KEY_NAME, "");

        return token;
    }

    @Override
    public void deleteToken() {
        SharedPreferences prefs;
        SharedPreferences.Editor edit;

        prefs = instance.getSharedPreferences(PREFERENCE_NAME, Context.MODE_PRIVATE);
        edit = prefs.edit();

        edit.remove(KEY_NAME);
        edit.apply();
    }

    @Override
    public boolean isAuth() {
        return !getToken().isEmpty();
    }
}
