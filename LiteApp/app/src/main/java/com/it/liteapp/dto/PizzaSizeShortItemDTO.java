package com.it.liteapp.dto;

public class PizzaSizeShortItemDTO {
    private SizeItemDTO size;
    private double price;

    public PizzaSizeShortItemDTO() {
    }

    public PizzaSizeShortItemDTO(SizeItemDTO size, double price) {
        this.size = size;
        this.price = price;
    }

    public SizeItemDTO getSize() {
        return size;
    }

    public double getPrice() {
        return price;
    }

    public void setSize(SizeItemDTO size) {
        this.size = size;
    }

    public void setPrice(double price) {
        this.price = price;
    }
}
