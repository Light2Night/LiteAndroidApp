package com.it.liteapp.dto;

import java.util.List;

public class PizzaItemDTO {
    private long id;
    private String name;
    private String description;
    private double rating;
    private Boolean isAvailable;
    private long categoryId;
    private List<PizzaImageShortItemDTO> images;
    private List<IngredientItemDTO> ingredients;
    private List<PizzaSizeShortItemDTO> sizes;

    public PizzaItemDTO() {
    }

    public PizzaItemDTO(long id, String name, String description, double rating, Boolean isAvailable, long categoryId, List<PizzaImageShortItemDTO> images, List<IngredientItemDTO> ingredients, List<PizzaSizeShortItemDTO> sizes) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.rating = rating;
        this.isAvailable = isAvailable;
        this.categoryId = categoryId;
        this.images = images;
        this.ingredients = ingredients;
        this.sizes = sizes;
    }

    public long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public double getRating() {
        return rating;
    }

    public Boolean getAvailable() {
        return isAvailable;
    }

    public long getCategoryId() {
        return categoryId;
    }

    public List<PizzaImageShortItemDTO> getImages() {
        return images;
    }

    public List<IngredientItemDTO> getIngredients() {
        return ingredients;
    }

    public List<PizzaSizeShortItemDTO> getSizes() {
        return sizes;
    }

    public void setId(long id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setRating(double rating) {
        this.rating = rating;
    }

    public void setAvailable(Boolean available) {
        isAvailable = available;
    }

    public void setCategoryId(long categoryId) {
        this.categoryId = categoryId;
    }

    public void setImages(List<PizzaImageShortItemDTO> images) {
        this.images = images;
    }

    public void setIngredients(List<IngredientItemDTO> ingredients) {
        this.ingredients = ingredients;
    }

    public void setSizes(List<PizzaSizeShortItemDTO> sizes) {
        this.sizes = sizes;
    }
}
