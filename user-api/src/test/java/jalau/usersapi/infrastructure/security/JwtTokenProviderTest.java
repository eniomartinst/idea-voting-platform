package jalau.usersapi.infrastructure.security;

import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class JwtTokenProviderTest {

    private final JwtTokenProvider provider = new JwtTokenProvider();

    @Test
    void shouldGenerateValidJwtToken() {
        User user = new User("1", "Javier", "jroca", "pass123");

        Token token = provider.generateToken(user);

        assertNotNull(token);
        assertNotNull(token.getAccessToken());
        assertEquals("Bearer", token.getTokenType());
        assertEquals(user, token.getUser());
    }
}