package com.ideavoting.usersapi.controllers;

import com.ideavoting.usersapi.models.LoginRequest;
import com.ideavoting.usersapi.models.Token;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.services.AuthService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
@RequiredArgsConstructor
public class AuthController {

    private final AuthService authService;

    @PostMapping("/login")
    public ResponseEntity<Token> login(@RequestBody LoginRequest request) {
        User user = User.builder()
                .login(request.getLogin())
                .password(request.getPassword())
                .build();
        Token token = authService.authenticate(user);
        return ResponseEntity.ok(token);
    }
}