package com.it.liteapp.dto;

public class SizeItemDTO {
    private long id;
    private String name;

    public SizeItemDTO() {
    }

    public SizeItemDTO(long id, String name) {
        this.id = id;
        this.name = name;
    }

    public long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public void setId(long id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }
}
