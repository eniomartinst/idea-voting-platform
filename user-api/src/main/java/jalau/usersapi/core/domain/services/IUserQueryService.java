package jalau.usersapi.core.domain.services;

import jalau.usersapi.core.domain.entities.User;
import java.util.List;
import java.util.concurrent.CompletableFuture;

/**
 * Service interface to query users.
 * Provides asynchronous and synchronous methods to access user data.
 */
public interface IUserQueryService {
    
    /**
     * Returns all users asynchronously.
     *
     * @return a CompletableFuture containing a list of users
     */
    CompletableFuture<List<User>> getUsers();
    
    CompletableFuture<User> getUser(String id);

    CompletableFuture<User> getUserByLogin(String login);
}
