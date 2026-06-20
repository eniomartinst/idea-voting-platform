package jalau.usersapi.core.domain.services;

import jalau.usersapi.core.domain.entities.User;

import java.util.concurrent.CompletableFuture;

/**
 * Defines command operations for managing users.
 * <p>
 * This interface represents the contract for write operations
 * such as creating, updating, and deleting users.
 */
public interface IUserCommandService {
    
    /**
     * Creates a new user asynchronously.
     *
     * @param user the user to be created
     * @return a {@link CompletableFuture} containing the created user
     */
    CompletableFuture<User> createUser(User user);
    
    /**
     * Updates an existing user.
     *
     * @param user the user to be updated
     * @return the updated user
     */
    CompletableFuture<User> updateUser(User user);
    
    /**
     * Deletes a user by its unique identifier.
     *
     * @param id the user ID
     */
    CompletableFuture<Void> deleteUser(String id);
}
