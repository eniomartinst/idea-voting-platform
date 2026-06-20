package jalau.usersapi.core.domain.services;
import jalau.usersapi.core.domain.entities.Token;
import jalau.usersapi.core.domain.entities.User;

public interface IAuthService { Token authenticate(User user); }