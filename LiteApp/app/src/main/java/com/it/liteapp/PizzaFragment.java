package com.it.liteapp;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.text.SpannableStringBuilder;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.it.liteapp.config.Config;
import com.it.liteapp.dto.IngredientItemDTO;
import com.it.liteapp.dto.PizzaItemDTO;
import com.it.liteapp.dto.PizzaSizeShortItemDTO;
import com.it.liteapp.network.RetrofitClient;

import org.jetbrains.annotations.NotNull;

import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PizzaFragment extends Fragment {
    private static final String ARG_ID = "id";
    private ImageView pizzaImage;
    private TextView pizzaName;
    private TextView pizzaDescription;
    private TextView pizzaRating;
    private TextView pizzaAvailability;
    private TextView pizzaIngredients;
    private TextView pizzaSizes;

    public static PizzaFragment newInstance(String id) {
        PizzaFragment fragment = new PizzaFragment();
        Bundle args = new Bundle();
        args.putString(ARG_ID, id);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_pizza, container, false);

        pizzaImage = view.findViewById(R.id.pizza_image);
        pizzaName = view.findViewById(R.id.pizza_name);
        pizzaDescription = view.findViewById(R.id.pizza_description);
        pizzaRating = view.findViewById(R.id.pizza_rating);
        pizzaAvailability = view.findViewById(R.id.pizza_availability);
        pizzaIngredients = view.findViewById(R.id.pizza_ingredients);
        pizzaSizes = view.findViewById(R.id.pizza_sizes);

        return view;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (getArguments() != null) {
            String id = getArguments().getString(ARG_ID);

            RetrofitClient
                    .getInstance()
                    .getPizzasApi()
                    .pizza(Long.parseLong(id))
                    .enqueue(new Callback<PizzaItemDTO>() {
                        @Override
                        public void onResponse(@NotNull Call<PizzaItemDTO> call, @NotNull Response<PizzaItemDTO> response) {
                            PizzaItemDTO item = response.body();
                            if (item != null) {
                                updateUI(item);
                            }
                        }

                        @Override
                        public void onFailure(@NotNull Call<PizzaItemDTO> call, @NotNull Throwable throwable) {

                        }
                    });
        }
    }

    private void updateUI(PizzaItemDTO item) {
        pizzaName.setText(item.getName());
        pizzaDescription.setText(getString(R.string.pizza_description_placeholder, item.getDescription()));
        pizzaRating.setText(getString(R.string.pizza_rating_placeholder, item.getRating()));
        pizzaAvailability.setText(item.getAvailable() ? R.string.available : R.string.not_available);

        SpannableStringBuilder ingredientsText = new SpannableStringBuilder();
        for (IngredientItemDTO ingredient : item.getIngredients()) {
            ingredientsText.append(" • ").append(ingredient.getName()).append("\n");
        }
        pizzaIngredients.setText(ingredientsText);

        StringBuilder sizesText = new StringBuilder();
        for (PizzaSizeShortItemDTO size : item.getSizes()) {
            sizesText.append(size.getSize().getName()).append(" - ").append(String.format(Locale.US, "%.2f", size.getPrice())).append(" грн.\n");
        }
        pizzaSizes.setText(sizesText.toString().trim());

        if (!item.getImages().isEmpty()) {
            String imageUrl = Config.BASE_URL + "images/1200_" + item.getImages().get(0).getName();

            Glide.with(this)
                    .load(imageUrl)
                    .placeholder(R.drawable.placeholder_image)
                    .error(R.drawable.error_image)
                    //.apply(new RequestOptions().override(400))
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .into(pizzaImage);
        }
    }
}