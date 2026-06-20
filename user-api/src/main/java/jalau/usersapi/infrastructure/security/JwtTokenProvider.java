package jalau.usersapi.infrastructure.security;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;
import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.security.TokenProvider;
import org.springframework.stereotype.Component;

import java.security.Key;
import java.util.Date;

@Component
public class JwtTokenProvider implements TokenProvider {

    // Chave secreta gerada em memória para assinar o JWT
    private final Key key = Keys.secretKeyFor(SignatureAlgorithm.HS256);

    @Override
    public Token generateToken(User user) {
        String jwt = Jwts.builder()
                .setSubject(user.getLogin())
                .claim("role", "User")
                .claim("userId", user.getId())
                .setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + 3600000)) // Expira em 1 hora
                .signWith(key)
                .compact();

        Token token = new Token();
        token.setAccessToken(jwt);
        token.setUser(user);

        return token;
    }
}