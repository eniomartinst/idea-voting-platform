package com.ideavoting.usersapi.services;

import com.ideavoting.usersapi.models.Token;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.security.JwtTokenProvider;
import com.ideavoting.usersapi.exceptions.InvalidUserException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AuthService {

    private final UserService userService;
    private final JwtTokenProvider tokenProvider;

    public Token authenticate(User user) {
        try {
            User storedUser = userService.getUserByLogin(user.getLogin()).join();
            if (storedUser.getPassword().equals(user.getPassword())) {
                return tokenProvider.generateToken(storedUser);
            }
            throw new InvalidUserException("Invalid credentials");
        } catch (Exception e) {
            throw new InvalidUserException("Invalid credentials");
        }
    }
}