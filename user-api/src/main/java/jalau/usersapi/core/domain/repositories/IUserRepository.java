package jalau.usersapi.core.domain.repositories;

import jalau.usersapi.core.domain.entities.User;
import java.util.List;

/**
 * Repository interface that defines operations for managing User entities.
 * Acts as an abstraction between the domain layer and data access layer.
 */
public interface IUserRepository {
    
    /**
     * Creates a new user.
     *
     * @param user the user to be created
     * @return the created user
     */
    User createUser(User user);
    
    /**
     * Updates an existing user.
     *
     * @param user the user to be updated
     * @return the updated user
     */
    User updateUser(User user);
    
    /**
     * Deletes a user by its unique identifier.
     *
     * @param id the user ID
     */
    void deleteUser(String id);
    
    /**
     * Retrieves all users.
     *
     * @return a list of users
     */
    List<User> getUsers();
    
    /**
     * Retrieves a user by its unique identifier.
     *
     * @param id the user ID
     * @return the user
     */
    User getUser(String id);

    User getUserByLogin(String login);
}
