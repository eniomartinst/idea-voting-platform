package com.ideavoting.usersapi.core.domain.security;

import com.ideavoting.usersapi.core.domain.entities.Token;
import com.ideavoting.usersapi.core.domain.entities.User;

public interface TokenProvider {
    Token generateToken(User user);
}