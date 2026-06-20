package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.security.TokenProvider;
import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.core.exception.InvalidUserException;
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
    private IUserQueryService userQueryService;

    @Mock
    private TokenProvider tokenProvider;

    @InjectMocks
    private AuthService authService;

    @Test
    void shouldReturnTokenWhenCredentialsAreValid() {
        User storedUser = new User("1", "Name", "login", "password");
        when(userQueryService.getUserByLogin("login")).thenReturn(CompletableFuture.completedFuture(storedUser));

        Token mockToken = new Token();
        mockToken.setAccessToken("mock-jwt-token");
        mockToken.setTokenType("Bearer");
        when(tokenProvider.generateToken(storedUser)).thenReturn(mockToken);

        User input = new User(null, null, "login", "password");
        Token token = authService.authenticate(input);

        assertNotNull(token.getAccessToken());
        assertEquals("Bearer", token.getTokenType());
        assertEquals("mock-jwt-token", token.getAccessToken());
    }

    @Test
    void shouldThrowWhenPasswordIsWrong() {
        User storedUser = new User("1", "Name", "login", "password");
        when(userQueryService.getUserByLogin("login")).thenReturn(CompletableFuture.completedFuture(storedUser));

        User input = new User(null, null, "login", "wrongpass");

        assertThrows(InvalidUserException.class, () -> authService.authenticate(input));
    }

    @Test
    void shouldThrowWhenUserNotFound() {
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new RuntimeException("Not found"));
        when(userQueryService.getUserByLogin("login")).thenReturn(future);

        User input = new User(null, null, "login", "wrongpass");

        assertThrows(InvalidUserException.class, () -> authService.authenticate(input));
    }
}