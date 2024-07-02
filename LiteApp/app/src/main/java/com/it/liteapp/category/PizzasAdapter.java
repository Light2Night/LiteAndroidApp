package com.it.liteapp.category;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.it.liteapp.PizzaFragment;
import com.it.liteapp.R;
import com.it.liteapp.config.Config;
import com.it.liteapp.dto.PizzaItemDTO;

import java.util.List;

public class PizzasAdapter extends RecyclerView.Adapter<PizzaCardViewHolder> {
    private final List<PizzaItemDTO> items;
    private Context context;

    public PizzasAdapter(List<PizzaItemDTO> items) {
        this.items = items;
    }

    @NonNull
    @Override
    public PizzaCardViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        View view = LayoutInflater
                .from(parent.getContext())
                .inflate(R.layout.pizza_view, parent, false);
        return new PizzaCardViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull PizzaCardViewHolder holder, int position) {
        if (items != null && position < items.size()) {
            PizzaItemDTO item = items.get(position);
            holder.getPizzaName().setText(item.getName());
            String imageUrl = Config.BASE_URL + "images/1200_" + item.getImages().get(0).getName();
            Glide.with(holder.itemView.getContext())
                    .load(imageUrl)
                    .placeholder(R.drawable.placeholder_image)
                    .error(R.drawable.error_image)
                    //.apply(new RequestOptions().override(400))
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .into(holder.getIvPizzaImage());

            holder.itemView.setOnClickListener(v -> {
                if (context instanceof FragmentActivity) {
                    FragmentActivity activity = (FragmentActivity) context;
                    Fragment productsFragment = PizzaFragment.newInstance(String.valueOf(item.getId()));
                    activity.getSupportFragmentManager().beginTransaction()
                            .replace(R.id.fragment_container, productsFragment)
                            .addToBackStack(null)
                            .commit();
                }
            });
        }
    }

    @Override
    public int getItemCount() {
        return items.size();
    }
}
