package com.ideavoting.usersapi.security;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;
import com.ideavoting.usersapi.models.Token;
import com.ideavoting.usersapi.models.User;
import org.springframework.stereotype.Component;

import java.security.Key;
import java.util.Date;

@Component
public class JwtTokenProvider {

    private final Key key = Keys.secretKeyFor(SignatureAlgorithm.HS256);

    public Token generateToken(User user) {
        String jwt = Jwts.builder()
                .setSubject(user.getLogin())
                .claim("role", "User")
                .claim("userId", user.getId())
                .setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + 3600000)) // 1 hora
                .signWith(key)
                .compact();

        Token token = new Token();
        token.setAccessToken(jwt);
        token.setUser(user);
        return token;
    }
}