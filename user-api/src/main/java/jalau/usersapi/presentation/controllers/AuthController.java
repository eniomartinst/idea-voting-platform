package com.ideavoting.usersapi.presentation.controllers;

import jakarta.validation.Valid;
import com.ideavoting.usersapi.core.domain.entities.Token;
import com.ideavoting.usersapi.core.domain.entities.User;
import com.ideavoting.usersapi.core.domain.services.IAuthService;
import com.ideavoting.usersapi.presentation.dtos.LoginRequest;
import com.ideavoting.usersapi.presentation.dtos.LoginResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@CrossOrigin(origins = "*")
@RestController
@RequestMapping("/api/auth")
@RequiredArgsConstructor
public class AuthController {

    private final IAuthService authService;

    @PostMapping("/login")
    public ResponseEntity<LoginResponse> login(@Valid @RequestBody LoginRequest loginRequest) {
        User user = new User();
        user.setLogin(loginRequest.getLogin());
        user.setPassword(loginRequest.getPassword());

        Token token = authService.authenticate(user);

        LoginResponse response = new LoginResponse();
        response.setAccessToken(token.getAccessToken());
        response.setTokenType(token.getTokenType());
        response.setRole(token.getRole());

        return ResponseEntity.ok(response);
    }
}