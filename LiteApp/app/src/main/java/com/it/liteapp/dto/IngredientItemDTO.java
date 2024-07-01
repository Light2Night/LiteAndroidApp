package com.it.liteapp.dto;

public class IngredientItemDTO {
    private long id;
    private String name;
    private String image;

    public IngredientItemDTO() {
    }

    public IngredientItemDTO(long id, String name, String image) {
        this.id = id;
        this.name = name;
        this.image = image;
    }

    public long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getImage() {
        return image;
    }

    public void setId(long id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setImage(String image) {
        this.image = image;
    }
}
