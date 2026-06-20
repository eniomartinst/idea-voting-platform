package jalau.usersapi.core.domain.security;

import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;

public interface TokenProvider {
    Token generateToken(User user);
}