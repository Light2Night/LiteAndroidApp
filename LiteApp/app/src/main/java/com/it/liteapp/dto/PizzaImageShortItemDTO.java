package com.it.liteapp.dto;

public class PizzaImageShortItemDTO {
    private String name;
    private int priority;

    public PizzaImageShortItemDTO() {
    }

    public PizzaImageShortItemDTO(String name, int priority) {
        this.name = name;
        this.priority = priority;
    }

    public String getName() {
        return name;
    }

    public int getPriority() {
        return priority;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPriority(int priority) {
        this.priority = priority;
    }
}
