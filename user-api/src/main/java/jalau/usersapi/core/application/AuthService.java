package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.security.TokenProvider;
import jalau.usersapi.core.domain.services.IAuthService;
import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.core.exception.InvalidUserException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AuthService implements IAuthService {

    private final IUserQueryService userQueryService;
    private final TokenProvider tokenProvider; // Depende da abstração!

    @Override
    public Token authenticate(User inputUser) {
        User storedUser = findAndValidateUser(inputUser);
        return tokenProvider.generateToken(storedUser);
    }

    private User findAndValidateUser(User inputUser) {
        User storedUser;
        try {
            storedUser = userQueryService.getUserByLogin(inputUser.getLogin()).join();
        } catch (Exception e) {
            throw new InvalidUserException("Invalid credentials");
        }

        if (!storedUser.getPassword().equals(inputUser.getPassword())) {
            throw new InvalidUserException("Invalid credentials");
        }
        return storedUser;
    }
}