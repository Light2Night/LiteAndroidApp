package com.it.liteapp.category;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.it.liteapp.R;
import com.it.liteapp.config.Config;
import com.it.liteapp.dto.CategoryItemDTO;

import java.util.List;

public class CategoriesAdapter extends RecyclerView.Adapter<CategoryCardViewHolder> {
    private List<CategoryItemDTO> items;

    public CategoriesAdapter(List<CategoryItemDTO> items) {
        this.items = items;
    }

    @NonNull
    @Override
    public CategoryCardViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater
                .from(parent.getContext())
                .inflate(R.layout.category_view, parent, false);
        return new CategoryCardViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull CategoryCardViewHolder holder, int position) {
        if (items != null && position < items.size()) {
            CategoryItemDTO item = items.get(position);
            holder.getCategoryName().setText(item.getName());
            String imageUrl = Config.BASE_URL + "images/1200_" + item.getImage();
            Glide.with(holder.itemView.getContext())
                    .load(imageUrl)
                    .placeholder(R.drawable.placeholder_image)
                    .error(R.drawable.error_image)
                    //.apply(new RequestOptions().override(400))
                    .into(holder.getIvCategoryImage());
        }
    }

    @Override
    public int getItemCount() {
        return items.size();
    }
}
