package com.ideavoting.usersapi.models;

import lombok.Data;

@Data
public class Token {
    private String accessToken;
    private String tokenType = "Bearer";
    private User user;
}