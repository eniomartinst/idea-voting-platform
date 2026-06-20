package jalau.usersapi.presentation.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IAuthService;
import jalau.usersapi.core.exception.InvalidUserException;
import jalau.usersapi.presentation.dtos.LoginRequest;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(AuthController.class)
class AuthControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private IAuthService authService;

    @MockitoBean
    private jalau.usersapi.infrastructure.mysql.mybatis.UserMyBatisMapper userMyBatisMapper;

    @Autowired
    private ObjectMapper objectMapper;

    @Test
    void shouldReturn200AndTokenOnValidLogin() throws Exception {
        Token mockToken = new Token();
        mockToken.setAccessToken("ey...");
        when(authService.authenticate(any(User.class))).thenReturn(mockToken);

        LoginRequest req = new LoginRequest();
        req.setLogin("valid");
        req.setPassword("valid");

        mockMvc.perform(post("/api/auth/login")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(req)))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.accessToken").value("ey..."));
    }

    @Test
    void shouldReturn401OnInvalidLogin() throws Exception {
        when(authService.authenticate(any(User.class))).thenThrow(new InvalidUserException("Invalid credentials"));

        LoginRequest req = new LoginRequest();
        req.setLogin("invalid");
        req.setPassword("invalid");

        mockMvc.perform(post("/api/auth/login")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(req)))
                .andExpect(status().isUnauthorized());
    }
}