package com.it.liteapp;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.it.liteapp.category.CategoriesAdapter;
import com.it.liteapp.category.PizzasAdapter;
import com.it.liteapp.dto.CategoryItemDTO;
import com.it.liteapp.dto.PizzasPageDTO;
import com.it.liteapp.network.RetrofitClient;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PizzasFragment extends Fragment {
    private static final String ARG_CATEGORY_ID = "category_id";
    RecyclerView rcPizzas;

    public static PizzasFragment newInstance(String categoryId) {
        PizzasFragment fragment = new PizzasFragment();
        Bundle args = new Bundle();
        args.putString(ARG_CATEGORY_ID, categoryId);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_pizzas, container, false);

        rcPizzas = view.findViewById(R.id.rcPizzas);
        rcPizzas.setHasFixedSize(true);
        rcPizzas.setLayoutManager(new GridLayoutManager(view.getContext(), 1, RecyclerView.VERTICAL, false));

        return view;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (getArguments() != null) {
            String categoryId = getArguments().getString(ARG_CATEGORY_ID);

            RetrofitClient
                    .getInstance()
                    .getPizzasApi()
                    .page()
                    .enqueue(new Callback<PizzasPageDTO>() {
                        @Override
                        public void onResponse(Call<PizzasPageDTO> call, Response<PizzasPageDTO> response) {
                            PizzasPageDTO item = response.body();
                            PizzasAdapter pa = new PizzasAdapter(item.getData());
                            rcPizzas.setAdapter(pa);
                        }

                        @Override
                        public void onFailure(Call<PizzasPageDTO> call, Throwable throwable) {

                        }
                    });
        }
    }
}