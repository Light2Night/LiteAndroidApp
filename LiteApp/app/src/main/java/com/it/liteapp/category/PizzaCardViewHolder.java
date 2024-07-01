package com.it.liteapp.category;

import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.it.liteapp.R;

public class PizzaCardViewHolder extends RecyclerView.ViewHolder {
    private TextView pizzaName;
    private ImageView ivPizzaImage;

    public PizzaCardViewHolder(@NonNull View itemView) {
        super(itemView);
        pizzaName = itemView.findViewById(R.id.pizzaName);
        ivPizzaImage = itemView.findViewById(R.id.ivPizzaImage);
    }

    public TextView getPizzaName() {
        return pizzaName;
    }

    public ImageView getIvPizzaImage() {
        return ivPizzaImage;
    }
}
