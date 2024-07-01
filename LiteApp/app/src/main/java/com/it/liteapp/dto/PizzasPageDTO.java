package com.it.liteapp.dto;

import java.util.List;

public class PizzasPageDTO {
    private List<PizzaItemDTO> data;
    private int pagesAvailable;
    private int itemsAvailable;

    public PizzasPageDTO() {
    }

    public PizzasPageDTO(List<PizzaItemDTO> data, int pagesAvailable, int itemsAvailable) {
        this.data = data;
        this.pagesAvailable = pagesAvailable;
        this.itemsAvailable = itemsAvailable;
    }

    public List<PizzaItemDTO> getData() {
        return data;
    }

    public int getPagesAvailable() {
        return pagesAvailable;
    }

    public int getItemsAvailable() {
        return itemsAvailable;
    }

    public void setData(List<PizzaItemDTO> data) {
        this.data = data;
    }

    public void setPagesAvailable(int pagesAvailable) {
        this.pagesAvailable = pagesAvailable;
    }

    public void setItemsAvailable(int itemsAvailable) {
        this.itemsAvailable = itemsAvailable;
    }
}
