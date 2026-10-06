package com.ideavoting.usersapi.services;

import com.ideavoting.usersapi.models.Token;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.security.JwtTokenProvider;
import com.ideavoting.usersapi.exceptions.InvalidUserException;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.concurrent.CompletableFuture;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
class AuthServiceTest {

    @Mock
    private UserService userService;

    @Mock
    private JwtTokenProvider tokenProvider;

    @InjectMocks
    private AuthService authService;

    @Test
    void shouldReturnTokenWhenCredentialsAreValid() {
        User storedUser = User.builder().id("1").name("Name").login("login").password("password").build();
        when(userService.getUserByLogin("login")).thenReturn(CompletableFuture.completedFuture(storedUser));

        Token mockToken = new Token();
        mockToken.setAccessToken("mock-jwt-token");
        mockToken.setTokenType("Bearer");
        when(tokenProvider.generateToken(storedUser)).thenReturn(mockToken);

        User input = User.builder().login("login").password("password").build();
        Token token = authService.authenticate(input);

        assertNotNull(token.getAccessToken());
        assertEquals("Bearer", token.getTokenType());
        assertEquals("mock-jwt-token", token.getAccessToken());
    }

    @Test
    void shouldThrowWhenPasswordIsWrong() {
        User storedUser = User.builder().id("1").name("Name").login("login").password("password").build();
        when(userService.getUserByLogin("login")).thenReturn(CompletableFuture.completedFuture(storedUser));

        User input = User.builder().login("login").password("wrongpass").build();

        assertThrows(InvalidUserException.class, () -> authService.authenticate(input));
    }

    @Test
    void shouldThrowWhenUserNotFound() {
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new RuntimeException("Not found"));
        when(userService.getUserByLogin("login")).thenReturn(future);

        User input = User.builder().login("login").password("wrongpass").build();

        assertThrows(InvalidUserException.class, () -> authService.authenticate(input));
    }
}