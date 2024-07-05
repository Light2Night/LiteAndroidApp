package com.it.liteapp;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.it.liteapp.application.HomeApplication;
import com.it.liteapp.security.JwtSecurityService;

public class AccountFragment extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_account, container, false);

        Button logoutButton = view.findViewById(R.id.account_logout_button);
        logoutButton.setOnClickListener(this::onClickLogOut);

        return view;
    }

    public void onClickLogOut(View v) {
        Context context = getActivity();

        if (context != null) {
            HomeApplication.getInstance().deleteToken();

            Intent intent = new Intent(context, AuthorizationActivity.class);

            intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);

            startActivity(intent);
            getActivity().finish();
        }
    }
}