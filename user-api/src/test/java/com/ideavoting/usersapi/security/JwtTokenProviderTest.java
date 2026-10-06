package com.ideavoting.usersapi.security;

import com.ideavoting.usersapi.models.Token;
import com.ideavoting.usersapi.models.User;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class JwtTokenProviderTest {

    private final JwtTokenProvider provider = new JwtTokenProvider();

    @Test
    void shouldGenerateValidJwtToken() {
        User user = User.builder().id("1").name("Javier").login("jroca").password("pass123").build();

        Token token = provider.generateToken(user);

        assertNotNull(token);
        assertNotNull(token.getAccessToken());
        assertEquals("Bearer", token.getTokenType());
        assertEquals(user, token.getUser());
    }
}